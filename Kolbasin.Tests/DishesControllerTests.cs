using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using Kolbasin.API.Controllers;
using Kolbasin.API.Data;
using Domain.Entities;
using Domain.Models;
using NSubstitute;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace Kolbasin.Tests
{
    public class DishesControllerTests : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        private readonly IWebHostEnvironment _environment;

        public DishesControllerTests()
        {
            _environment = Substitute.For<IWebHostEnvironment>();
            // Create and open a connection. This creates the SQLite in-memory database,
            // which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite,
            // including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            using var context = new AppDbContext(_contextOptions);
            context.Database.EnsureCreated();
            
            var categories = new Category[]
            {
                new Category {Id=1, Name="Супы", NormalizedName="soups"},
                new Category {Id=2, Name="Основные блюда", NormalizedName="main-dishes"}
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();

            var dishes = new List<Dish>
            {
                new Dish {Id=1, Name="Борщ", Description="", Price=15, Weight=300, CategoryId=1, Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("soups"))},
                new Dish {Id=2, Name="Харчо", Description="", Price=20, Weight=250, CategoryId=1, Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("soups"))},
                new Dish {Id=3, Name="Стейк", Description="", Price=50, Weight=300, CategoryId=2, Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("main-dishes"))},
                new Dish {Id=4, Name="Паста", Description="", Price=30, Weight=250, CategoryId=2, Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("main-dishes"))},
                new Dish {Id=5, Name="Пицца", Description="", Price=40, Weight=400, CategoryId=2, Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("main-dishes"))}
            };
            context.Dishes.AddRange(dishes);
            context.SaveChanges();
        }

        public void Dispose() => _connection?.Dispose();

        AppDbContext CreateContext() => new AppDbContext(_contextOptions);

        // Проверка фильтра по категории
        [Fact]
        public async Task ControllerFiltersCategory()
        {
            // arrange
            using var context = CreateContext();
            var category = context.Categories.First();
            var controller = new DishesController(context, _environment);
            // act
            var response = await controller.GetDishes(category.NormalizedName);
            var responseData = response.Value;
            var dishesList = responseData?.Data?.Items; // полученный список объектов
            //assert
            Assert.NotNull(dishesList);
            Assert.True(dishesList.All(d => d.CategoryId == category.Id));
        }

        // Проверка подсчета количества страниц
        // Первый параметр - размер страницы
        // Второй параметр - ожидаемое количество страниц (при условии, что всего объектов 5)
        [Theory]
        [InlineData(2, 3)]
        [InlineData(3, 2)]
        public async Task ControllerReturnsCorrectPagesCount(int size, int qty)
        {
            using var context = CreateContext();
            var controller = new DishesController(context, _environment);
            // act
            var response = await controller.GetDishes(null, 1, size);
            var responseData = response.Value;
            var totalPages = responseData?.Data?.TotalPages; // полученное количество страниц
            //assert
            Assert.Equal(qty, totalPages); // количество страниц совпадает
        }

        [Fact]
        public async Task ControllerReturnsCorrectPage()
        {
            using var context = CreateContext();
            var controller = new DishesController(context, _environment);
            // При размере страницы 3 и общем количестве объектов 5
            // на 2-й странице должно быть 2 объекта
            // Первый объект на второй странице
            Dish firstItem = context.Dishes.ToArray()[3];
            // act
            // Получить данные 2-й страницы
            var response = await controller.GetDishes(null, 2, 3);
            var responseData = response.Value;
            var dishesList = responseData?.Data?.Items; // полученный список объектов
            var currentPage = responseData?.Data?.CurrentPage; // полученный номер текущей страницы
            //assert
            Assert.NotNull(dishesList);
            Assert.NotNull(currentPage);
            Assert.Equal(2, currentPage);// номер страницы совпадает
            Assert.Equal(2, dishesList.Count); // количество объектов на странице равно 2
            Assert.Equal(firstItem.Id, dishesList[0].Id); // 1-й объект в списке правильный
        }
    }
}

