using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kolbasin.API.Data
{
    public static class DbInitializer
    {
        public static async Task SeedDataAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            
            // Создаем базу данных, если она не существует
            await context.Database.EnsureCreatedAsync();

            // Проверяем, есть ли уже данные
            var hasData = await context.Categories.AnyAsync();

            var categories = new[]
            {
                new Category {Name="Стартеры",NormalizedName="starters"},
                new Category {Name="Салаты", NormalizedName="salads"},
                new Category {Name="Супы", NormalizedName="soups"},
                new Category {Name="Основные блюда", NormalizedName="maincourses"},
                new Category {Name="Десерты", NormalizedName="desserts"},
                new Category {Name="Напитки", NormalizedName="drinks"},
            };

            if (!hasData)
            {
                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // Сохраняем Id категорий в словарь для безопасного доступа
            var categoryIds = (await context.Categories.ToListAsync()).ToDictionary(c => c.NormalizedName, c => c.Id);
            var dishes = new[]
            { new Dish
                {
                    Name = "Суп-харчо",
                    Description = "Острый суп с говядиной и рисом",
                    Weight = 200,
                    Image = "img/dishes/Харчо.webp",
                    Price = 15,
                    CategoryId = categoryIds["soups"]
                },
                new Dish
                {
                    Name = "Борщ",
                    Description = "Без сметаны",
                    Price = 15,
                    Weight = 330,
                    Image = "img/dishes/Борщ.jpeg",
                    CategoryId = categoryIds["soups"]
                },
                new Dish
                {
                    Name = "Цезарь",
                    Description = "Классический салат Цезарь с добавлением куриного филе",
                    Price = 25,
                    Weight = 150,
                    Image = "img/dishes/Цезарь.jpeg",
                    CategoryId = categoryIds["salads"]
                },
                new Dish
                {
                    Name = "Оливье",
                    Description = "Без горошка",
                    Price = 12,
                    Weight = 200,
                    Image = "img/dishes/Оливье.jpeg",
                    CategoryId = categoryIds["salads"]
                },
                new Dish
                {
                    Name = "Брускетта",
                    Description = "С авокадо и креветкой",
                    Price = 16,
                    Weight = 120,
                    Image = "img/dishes/Брускетта.jpg",
                    CategoryId = categoryIds["starters"]
                },
                new Dish
                {
                    Name = "Крылышки Баффало",
                    Description = "Очень острые",
                    Price = 26,
                    Weight = 250,
                    Image = "img/dishes/Крылышки.jpeg",
                    CategoryId = categoryIds["starters"]
                },

                new Dish
                {
                    Name = "Лагман",
                    Description = "С говядиной и овощами",
                    Weight = 300,
                    Image = "img/dishes/Лагман.jpg",
                    Price = 20,
                    CategoryId = categoryIds["soups"]
                },
                new Dish
                {
                    Name = "Греческий салат",
                    Description = "Со свежими овощами и фетой",
                    Price = 150,
                    Weight = 180,
                    Image = "img/dishes/Греческий.webp",
                    CategoryId = categoryIds["salads"]
                },
                new Dish
                {
                    Name = "Креветки в кляре",
                    Description = "С соусом тартар",
                    Price = 25,
                    Weight = 220,
                    Image = "img/dishes/Креветки.jpeg",
                    CategoryId = categoryIds["starters"]
                },
                new Dish
                {
                    Name = "Том Ям",
                    Description = "Острый тайский суп с кокосовым молоком",
                    Price = 30,
                    Weight = 280,
                    Image = "img/dishes/ТомЯм.webp",
                    CategoryId = categoryIds["soups"]
                },
                new Dish
                {
                    Name = "Салат с тунцом",
                    Description = "Свежий салат с консервированным тунцом и овощами",
                    Price = 20,
                    Weight = 190,
                    Image = "img/dishes/СалатСТунцом.jpg",
                    CategoryId = categoryIds["salads"]
                },
                new Dish
                {
                    Name = "Мини-бургеры",
                    Description = "Небольшие бургеры с говяжьей котлетой и сыром",
                    Price = 23,
                    Weight = 150,
                    Image = "img/dishes/МиниБургеры.jpeg",
                    CategoryId = categoryIds["starters"]
                },
                new Dish
                {
                    Name = "Мисо-суп",
                    Description = "Японский суп с тофу и водорослями",
                    Price = 15,
                    Weight = 250,
                    Image = "img/dishes/МисоСуп.webp",
                    CategoryId = categoryIds["soups"]
                },
                new Dish
                {
                    Name = "Пина Колада",
                    Description = "Безалкогольный коктейль с ананасовым соком и кокосовым молоком",
                    Price = 13,
                    Weight = 200,
                    Image = "img/dishes/ПинаКолада.jpg",
                    CategoryId = categoryIds["drinks"]
                },
                new Dish
                {
                    Name = "Мохито",
                    Description = "Безалкогольный освежающий коктейль с мятой и лаймом",
                    Price = 13,
                    Weight = 200,
                    Image = "img/dishes/Мохито.webp",
                    CategoryId = categoryIds["drinks"]
                },
                new Dish
                {
                    Name = "Чизкейк",
                    Description = "Классический чизкейк с клубничным соусом",
                    Price = 15,
                    Weight = 150,
                    Image = "img/dishes/Чизкейк.webp",
                    CategoryId = categoryIds["desserts"]
                },
                new Dish
                {
                    Name = "Тирамису",
                    Description = "Итальянский десерт с маскарпоне и кофе",
                    Price = 16,
                    Weight = 150,
                    Image = "img/dishes/Тирамису.jpg",
                    CategoryId = categoryIds["desserts"]
                },
                new Dish
                {
                    Name = "Крем-суп из тыквы",
                    Description = "Сливочный суп с тыквой и специями",
                    Price = 13,
                    Weight = 300,
                    Image = "img/dishes/КремСупИзТыквы.jpg",
                    CategoryId = categoryIds["soups"]
                },
                new Dish
                {
                    Name = "Лимонад",
                    Description = "Домашний лимонад с мятой и лимоном",
                    Price = 11,
                    Weight = 250,
                    Image = "img/dishes/Лимонад.jpeg",
                    CategoryId = categoryIds["drinks"]
                },
                new Dish
                {
                    Name = "Наполеон",
                    Description = "Слоеный торт с заварным кремом",
                    Price = 13,
                    Weight = 180,
                    Image = "img/dishes/Наполеон.jpeg",
                    CategoryId = categoryIds["desserts"]
                },
                new Dish
                {
                    Name = "Айс-ти",
                    Description = "Освежающий холодный чай с лимоном",
                    Price = 10,
                    Weight = 250,
                    Image = "img/dishes/АйсТи.jpg",
                    CategoryId = categoryIds["drinks"]
                }
            };
            
            if (!hasData)
            {
                await context.Dishes.AddRangeAsync(dishes);
                await context.SaveChangesAsync();
            }

            // Обновляем пути к изображениям для существующих блюд, если они отсутствуют
            await UpdateMissingImagePathsAsync(context);
        }

        private static async Task UpdateMissingImagePathsAsync(AppDbContext context)
        {
            // Словарь соответствия названий блюд и путей к изображениям
            var imagePaths = new Dictionary<string, string>
            {
                { "Суп-харчо", "img/dishes/Харчо.webp" },
                { "Борщ", "img/dishes/Борщ.jpeg" },
                { "Салат с тунцом", "img/dishes/СалатСТунцом.jpg" },
                { "Цезарь", "img/dishes/Цезарь.jpeg" },
                { "Оливье", "img/dishes/Оливье.jpeg" },
                { "Брускетта", "img/dishes/Брускетта.jpg" },
                { "Крылышки Баффало", "img/dishes/Крылышки.jpeg" },
                { "Лагман", "img/dishes/Лагман.jpg" },
                { "Греческий салат", "img/dishes/Греческий.webp" },
                { "Креветки в кляре", "img/dishes/Креветки.jpeg" },
                { "Том Ям", "img/dishes/ТомЯм.webp" },
                { "Мини-бургеры", "img/dishes/МиниБургеры.jpeg" },
                { "Мисо-суп", "img/dishes/МисоСуп.webp" },
                { "Пина Колада", "img/dishes/ПинаКолада.jpg" },
                { "Мохито", "img/dishes/Мохито.webp" },
                { "Чизкейк", "img/dishes/Чизкейк.webp" },
                { "Тирамису", "img/dishes/Тирамису.jpg" },
                { "Крем-суп из тыквы", "img/dishes/КремСупИзТыквы.jpg" },
                { "Лимонад", "img/dishes/Лимонад.jpeg" },
                { "Наполеон", "img/dishes/Наполеон.jpeg" },
                { "Айс-ти", "img/dishes/АйсТи.jpg" }
            };

            var dishes = await context.Dishes.ToListAsync();
            bool hasChanges = false;

            foreach (var dish in dishes)
            {
                // Если путь к изображению отсутствует или пустой, и есть соответствие в словаре
                if ((string.IsNullOrEmpty(dish.Image) || 
                     dish.Image.StartsWith("http://localhost:9999/", StringComparison.OrdinalIgnoreCase) ||
                     dish.Image.StartsWith("https://localhost:9999/", StringComparison.OrdinalIgnoreCase)) &&
                    imagePaths.TryGetValue(dish.Name, out var imagePath))
                {
                    // Если это полный URL, извлекаем относительный путь
                    if (dish.Image != null && 
                        (dish.Image.StartsWith("http://localhost:9999/", StringComparison.OrdinalIgnoreCase) ||
                         dish.Image.StartsWith("https://localhost:9999/", StringComparison.OrdinalIgnoreCase)))
                    {
                        var uri = new Uri(dish.Image);
                        dish.Image = uri.PathAndQuery.TrimStart('/');
                    }
                    else
                    {
                        dish.Image = imagePath;
                    }
                    hasChanges = true;
                }
            }

            if (hasChanges)
            {
                await context.SaveChangesAsync();
            }
        }
    }
}