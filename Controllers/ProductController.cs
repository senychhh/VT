using Microsoft.AspNetCore.Mvc;
using Kolbasin_lab1.Services;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Components;
using System.Data.Common;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;


namespace Kolbasin_lab1.Controllers
{
    public class ProductController : Controller
    {
        

        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
      

        public ProductController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        // GET: /Product
        public async Task<IActionResult> Index(string? category, int page = 1)
        {
            // Получаем список блюд с пагинацией
            var dishesResponse = await _productService.GetProductListAsync(category, page);

            // Получаем список категорий
            var categoriesResponse = await _categoryService.GetCategoryListAsync();

            // Передаем данные через ViewBag
            ViewBag.Categories = categoriesResponse?.Data ?? new List<Category>();
            ViewBag.SelectedCategory = category;

            // Передаем ProductListModel<Dish> как модель
            return View(dishesResponse.Data);
        }
    }
}
