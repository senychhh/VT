using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Kolbasin_lab1.Data;
using Kolbasin_lab1.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Kolbasin_lab1.Areas.Admin.Pages.Dishes
{
    [Authorize(Policy = "admin")]
    
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService;
        }

        public IList<Dish> Dishes { get; set; } = new List<Dish>();

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public async Task OnGetAsync(int pageNo = 1)
        {
            var response = await _productService.GetProductListAsync(null, pageNo);

            if (response.Success)
            {
                Dishes = response.Data.Items;
                CurrentPage = response.Data.CurrentPage;
                TotalPages = response.Data.TotalPages;
            }
        }
    }
}