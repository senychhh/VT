using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Domain.Entities;
using Kolbasin_lab1.Data;
using Microsoft.AspNetCore.Authorization;
using Kolbasin_lab1.Services;

namespace Kolbasin_lab1.Areas.Admin.Pages.Dishes
{
    [Authorize(Policy = "admin")]
    public class DetailsModel : PageModel
    {
        private readonly IProductService _productService;

        public DetailsModel(IProductService productService)
        {
            _productService = productService;
        }

        public Dish Dish { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _productService.GetProductByIdAsync(id.Value);
            
            if (!response.Success || response.Data == null)
            {
                return NotFound();
            }

            Dish = response.Data;
            return Page();
        }
    }
}
