using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Domain.Entities;
using Kolbasin_lab1.Data;
using Microsoft.AspNetCore.Authorization;
using Kolbasin_lab1.Services;

namespace Kolbasin_lab1.Areas.Admin.Pages.Dishes
{
    [Authorize(Policy = "admin")]
    public class CreateModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        

        public CreateModel(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [BindProperty]
        public Dish Dish { get; set; } = new();

        [BindProperty]
        public IFormFile? Image { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var categoryListData = await _categoryService.GetCategoryListAsync();
            ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Перезагружаем список категорий при ошибке валидации
                var categoryListData = await _categoryService.GetCategoryListAsync();
                ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id", "Name");
                return Page();
            }

            var response = await _productService.CreateProductAsync(Dish, Image);
            if (!response.Success)
            {
                ModelState.AddModelError("", response.ErrorMessage ?? "Ошибка при создании блюда");
                // Перезагружаем список категорий при ошибке
                var categoryListData = await _categoryService.GetCategoryListAsync();
                ViewData["CategoryId"] = new SelectList(categoryListData.Data, "Id", "Name");
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}