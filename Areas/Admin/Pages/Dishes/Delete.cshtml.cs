using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Kolbasin_lab1.Data;
using Microsoft.AspNetCore.Authorization;
using Kolbasin_lab1.Services;

namespace Kolbasin_lab1.Areas.Admin.Pages.Dishes
{
    [Authorize(Policy = "admin")]
    public class DeleteModel : PageModel
    {
        private readonly IProductService _productService;

        public DeleteModel(IProductService productService)
        {
            _productService = productService;
        }

        [BindProperty]
        public Dish Dish { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            var response = await _productService.GetProductByIdAsync(id.Value);
            if (!response.Success || response.Data == null) return NotFound();

            Dish = response.Data;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
{
    if (Dish.Id == 0) return NotFound();

    try
    {
        var response = await _productService.DeleteProductAsync(Dish.Id);
        if (!response.Success)
        {
            ModelState.AddModelError("", response.ErrorMessage ?? "Ошибка при удалении");
            // Перезагружаем данные блюда для отображения
            var dishResponse = await _productService.GetProductByIdAsync(Dish.Id);
            if (dishResponse.Success && dishResponse.Data != null)
            {
                Dish = dishResponse.Data;
            }
            return Page();
        }
        return RedirectToPage("./Index");
    }
    catch (Exception ex)
    {
        // Логируем ошибку и возвращаем страницу с сообщением
        Console.WriteLine(ex.Message);
        ModelState.AddModelError("", "Ошибка при удалении: " + ex.Message);
        return Page();
    }
}
    }
}