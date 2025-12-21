using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Kolbasin.API.Data;
using Domain.Models;

namespace Kolbasin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DishesController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        /// GET: api/Dishes
        [HttpGet]
        public async Task<ActionResult<ResponseData<ProductListModel<Dish>>>> GetDishes(
            string? category,
            int pageNo = 1,
            int pageSize = 3)
            
        {
            // объект результата
            var result = new ResponseData<ProductListModel<Dish>>();

            // базовый запрос - исключаем удаленные блюда
            var query = _context.Dishes
                .Include(d => d.Category)
                .Where(d => !d.IsDeleted && (string.IsNullOrEmpty(category)
                    || d.Category != null &&  d.Category.NormalizedName == category));

            // общее количество страниц
            int totalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);

            if (pageNo > totalPages && totalPages > 0)
                pageNo = totalPages;

            // данные текущей страницы
            var items = await query
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            result.Data = new ProductListModel<Dish>
            {
                Items = items,
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            if (items.Count == 0)
            {
                result.Success = false;
                result.ErrorMessage = "Нет объектов в выбранной категории";
            }

            return Ok(result);
        }


        // GET: api/Dishes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);

            if (dish == null || dish.IsDeleted)
            {
                return NotFound();
            }

            return dish;
        }

        // PUT: api/Dishes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            if (id != dish.Id)
            {
                return BadRequest();
            }

            // Сохраняем старые значения важных полей
            var existingDish = await _context.Dishes.FindAsync(id);
            if (existingDish == null || existingDish.IsDeleted)
            {
                return NotFound();
            }

            // Сохраняем старое значение Image, если новое пустое
            if (string.IsNullOrEmpty(dish.Image))
            {
                dish.Image = existingDish.Image;
            }

            // Сохраняем значения IsDeleted и DeletedAt (не позволяем изменять их через PUT)
            dish.IsDeleted = existingDish.IsDeleted;
            dish.DeletedAt = existingDish.DeletedAt;

            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

 [HttpPost("{id}")]
        public async Task<IActionResult> SaveImage(int id, IFormFile image)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null) return NotFound();

            if (image == null || image.Length == 0)
                return BadRequest("Файл не передан");

            // если было старое изображение — удалим файл
            TryDeleteImageFile(dish.Image);

            // Сохраняем в img/dishes/ вместо Images/
            var imagesPath = Path.Combine(_env.WebRootPath, "img", "dishes");
            Directory.CreateDirectory(imagesPath);

            var randomName = Path.GetRandomFileName();
            var extension = Path.GetExtension(image.FileName);
            var fileName = Path.ChangeExtension(randomName, extension);
            var filePath = Path.Combine(imagesPath, fileName);

            using (var stream = System.IO.File.OpenWrite(filePath))
            {
                await image.CopyToAsync(stream);
            }

            // Сохраняем относительный путь вместо полного URL
            var relativePath = $"img/dishes/{fileName}";
            dish.Image = relativePath;

            await _context.SaveChangesAsync();
            return Ok(relativePath);
        }

        // POST: api/Dishes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dish>> PostDish(Dish dish)
        {
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDish", new { id = dish.Id }, dish);
        }

        // DELETE: api/Dishes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null || dish.IsDeleted)
            {
                return NotFound();
            }

            // Мягкое удаление - помечаем как удаленное вместо физического удаления
            dish.IsDeleted = true;
            dish.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Dishes/deleted - получить список удаленных блюд
        [HttpGet("deleted")]
        public async Task<ActionResult<ResponseData<ProductListModel<Dish>>>> GetDeletedDishes(
            int pageNo = 1,
            int pageSize = 10)
        {
            var result = new ResponseData<ProductListModel<Dish>>();

            // Запрос только удаленных блюд
            var query = _context.Dishes
                .Include(d => d.Category)
                .Where(d => d.IsDeleted)
                .OrderByDescending(d => d.DeletedAt);

            int totalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize);

            if (pageNo > totalPages && totalPages > 0)
                pageNo = totalPages;

            var items = await query
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            result.Data = new ProductListModel<Dish>
            {
                Items = items,
                CurrentPage = pageNo,
                TotalPages = totalPages
            };

            return Ok(result);
        }

        // POST: api/Dishes/{id}/restore - восстановить удаленное блюдо
        [HttpPost("{id}/restore")]
        public async Task<IActionResult> RestoreDish(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null || !dish.IsDeleted)
            {
                return NotFound();
            }

            dish.IsDeleted = false;
            dish.DeletedAt = null;

            await _context.SaveChangesAsync();

            return Ok(dish);
        }

        private bool DishExists(int id)
        {
            return _context.Dishes.Any(e => e.Id == id);
        }
           private void TryDeleteImageFile(string? imageUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imageUrl)) return;

                string? fileName = null;
                string? folderPath = null;

                // Проверяем, это полный URL или относительный путь
                if (imageUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) || 
                    imageUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                {
                    // Полный URL - извлекаем путь
                    var uri = new Uri(imageUrl);
                    var pathAndQuery = uri.PathAndQuery.TrimStart('/');
                    
                    // Проверяем разные варианты путей
                    if (pathAndQuery.StartsWith("Images/", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName = Path.GetFileName(pathAndQuery.Substring("Images/".Length));
                        folderPath = Path.Combine(_env.WebRootPath, "Images");
                    }
                    else if (pathAndQuery.StartsWith("img/dishes/", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName = Path.GetFileName(pathAndQuery.Substring("img/dishes/".Length));
                        folderPath = Path.Combine(_env.WebRootPath, "img", "dishes");
                    }
                }
                else
                {
                    // Относительный путь
                    if (imageUrl.StartsWith("img/dishes/", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName = Path.GetFileName(imageUrl.Substring("img/dishes/".Length));
                        folderPath = Path.Combine(_env.WebRootPath, "img", "dishes");
                    }
                    else if (imageUrl.StartsWith("Images/", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName = Path.GetFileName(imageUrl.Substring("Images/".Length));
                        folderPath = Path.Combine(_env.WebRootPath, "Images");
                    }
                    else if (imageUrl.StartsWith("/img/dishes/", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName = Path.GetFileName(imageUrl.Substring("/img/dishes/".Length));
                        folderPath = Path.Combine(_env.WebRootPath, "img", "dishes");
                    }
                    else if (imageUrl.StartsWith("/Images/", StringComparison.OrdinalIgnoreCase))
                    {
                        fileName = Path.GetFileName(imageUrl.Substring("/Images/".Length));
                        folderPath = Path.Combine(_env.WebRootPath, "Images");
                    }
                }

                if (fileName != null && folderPath != null)
                {
                    // защита от ".."
                    fileName = Path.GetFileName(fileName);
                    var path = Path.Combine(folderPath, fileName);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
            }
            catch
            {
                // ignored
            }
    }
}
}
