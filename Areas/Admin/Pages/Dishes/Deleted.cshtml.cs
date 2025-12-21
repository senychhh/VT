using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Entities;
using Kolbasin_lab1.Services;
using Microsoft.AspNetCore.Authorization;

namespace Kolbasin_lab1.Areas.Admin.Pages.Dishes
{
    [Authorize(Policy = "admin")]
    public class DeletedModel : PageModel
    {
        private readonly IProductService _productService;

        public DeletedModel(IProductService productService)
        {
            _productService = productService;
        }

        public IList<Dish> DeletedDishes { get; set; } = new List<Dish>();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int pageNo = 1)
        {
            var response = await _productService.GetDeletedProductsAsync(pageNo);

            if (response.Success && response.Data != null)
            {
                DeletedDishes = response.Data.Items;
                CurrentPage = response.Data.CurrentPage;
                TotalPages = response.Data.TotalPages;
            }
        }

        public async Task<IActionResult> OnPostRestoreAsync(int id)
        {
            var response = await _productService.RestoreProductAsync(id);
            
            if (response.Success)
            {
                return RedirectToPage("./Deleted");
            }

            ModelState.AddModelError("", response.ErrorMessage ?? "Ошибка при восстановлении блюда");
            return Page();
        }
    }
}

