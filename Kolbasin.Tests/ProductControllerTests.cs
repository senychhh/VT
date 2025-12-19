using Xunit;
using Microsoft.AspNetCore.Mvc;
using Kolbasin_lab1.Controllers;
using Kolbasin_lab1.Services;
using Domain.Entities;
using Domain.Models;
using NSubstitute;

namespace Kolbasin.Tests
{
    public class ProductControllerTests
    {
        IProductService _productService = null!;
        ICategoryService _categoryService = null!;

        public ProductControllerTests()
        {
            SetupData();
        }

        // Список категорий сохраняется во ViewBag
        [Fact]
        public async Task IndexPutsCategoriesToViewData()
        {
            //arrange
            var controller = new ProductController(_productService, _categoryService);
            //act
            var response = await controller.Index(null);
            //assert
            var view = Assert.IsType<ViewResult>(response);
            var categories = Assert.IsType<List<Category>>(controller.ViewBag.Categories);
            Assert.Equal(6, categories.Count);
            Assert.Null(controller.ViewBag.SelectedCategory);
        }

        // Имя текущей категории сохраняется во ViewBag
        [Fact]
        public async Task IndexSetsCorrectCurrentCategory()
        {
            //arrange
            var categories = await _categoryService.GetCategoryListAsync();
            var currentCategory = categories.Data![0];
            var controller = new ProductController(_productService, _categoryService);
            //act
            var response = await controller.Index(currentCategory.NormalizedName);
            //assert
            var view = Assert.IsType<ViewResult>(response);
            Assert.Equal(currentCategory.NormalizedName, controller.ViewBag.SelectedCategory);
        }

        // В случае ошибки возвращается пустой список категорий
        [Fact]
        public async Task IndexHandlesError()
        {
            //arrange
            string errorMessage = "Test error";
            var categoriesResponse = new ResponseData<List<Category>>();
            categoriesResponse.Success = false;
            categoriesResponse.ErrorMessage = errorMessage;
            categoriesResponse.Data = null;
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(categoriesResponse));

            var controller = new ProductController(_productService, _categoryService);
            //act
            var response = await controller.Index(null);
            //assert
            var view = Assert.IsType<ViewResult>(response);
            var categories = Assert.IsType<List<Category>>(controller.ViewBag.Categories);
            Assert.Empty(categories); // При ошибке возвращается пустой список
        }

        // Настройка имитации ICategoryService и IProductService
        void SetupData()
        {
            _categoryService = Substitute.For<ICategoryService>();
            var categoriesResponse = new ResponseData<List<Category>>();
            categoriesResponse.Data = new List<Category>
            {
                new Category {Id=1, Name="Стартеры", NormalizedName="starters"},
                new Category {Id=2, Name="Салаты", NormalizedName="salads"},
                new Category {Id=3, Name="Супы", NormalizedName="soups"},
                new Category {Id=4, Name="Основные блюда", NormalizedName="main-dishes"},
                new Category {Id=5, Name="Напитки", NormalizedName="drinks"},
                new Category {Id=6, Name="Десерты", NormalizedName="desserts"}
            };
            categoriesResponse.Success = true;
            _categoryService.GetCategoryListAsync().Returns(Task.FromResult(categoriesResponse));

            _productService = Substitute.For<IProductService>();
            var dishes = new List<Dish>
            {
                new Dish {Id = 1 },
                new Dish { Id = 2 },
                new Dish { Id = 3 },
                new Dish { Id = 4 },
                new Dish { Id = 5 }
            };
            var productResponse = new ResponseData<ProductListModel<Dish>>();
            productResponse.Data = new ProductListModel<Dish> { Items = dishes };
            productResponse.Success = true;
            _productService.GetProductListAsync(Arg.Any<string?>(), Arg.Any<int>())
                .Returns(Task.FromResult(productResponse));
        }
    }
}

