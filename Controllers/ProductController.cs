using Microsoft.AspNetCore.Mvc;
using Kolbasin_lab1.Services;
using Domain.Entities;


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
     public async Task<IActionResult> Index(string? category)
{
    // Получаем список блюд
    var dishesResponse = await _productService.GetProductListAsync(category);
    
    // Получаем список категорий
    var categoriesResponse = await _categoryService.GetCategoryListAsync();

    // Передаем данные через ViewBag
    ViewBag.Categories = categoriesResponse?.Data ?? new List<Category>();
    ViewBag.SelectedCategory = category;

    // Передаем список блюд как модель
    var dishes = dishesResponse?.Data?.Items ?? new List<Dish>();
    return View(dishes);
}

    }
}
