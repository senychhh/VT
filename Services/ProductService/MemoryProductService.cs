using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Kolbasin_lab1.Services
{
        public class MemoryProductService : IProductService
        {
                List<Dish> _dishes;
                List<Category> _categories;
                public MemoryProductService(ICategoryService categoryService)
                {
                        _categories = categoryService.GetCategoryListAsync()
                        .Result
                        .Data;
                        SetupData();
                }
                /// Инициализация списков
                /// </summary>
                private void SetupData()
                {
                        _dishes = new List<Dish>
        {
            new Dish {Id = 1, Name="Суп-харчо",
                    Description="Острый суп с говядиной и рисом",
                    Weight= 200, Image="img/dishes/Харчо.webp",
                    Price=15,
                    CategoryId= _categories.Find(c=>c.NormalizedName.Equals("soups")).Id},
            new Dish { Id = 2, Name="Борщ",
                    Description="Без сметаны",
                    Price=15,
                    Weight= 330, Image="img/dishes/Борщ.jpeg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("soups")).Id},

            new Dish { Id = 3, Name="Цезарь",
                    Description="Классический салат Цезарь с добавлением куриного филе",
                    Price=25,
                    Weight=150, Image="img/dishes/Цезарь.jpeg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("salads")).Id},
            new Dish { Id = 4, Name="Оливье",
                    Description="Без горошка",
                    Price=12,
                    Weight=200, Image="img/dishes/Оливье.jpeg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("salads")).Id},

            new Dish { Id = 5, Name="Брускетта",
                    Description="С авокадо и креветкой",
                    Price=16,
                    Weight=120, Image="img/dishes/Брускетта.jpg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("starters")).Id},
            new Dish { Id = 6, Name="Крылышки Баффало",
                    Description="Очень острые",
                    Price=26,
                    Weight=250, Image="img/dishes/Крылышки.jpeg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("starters")).Id},

            new Dish { Id = 7, Name="Лагман",
                    Description="С говядиной и овощами",
                    Weight=300, Image="img/dishes/Лагман.jpg",
                    Price=20,
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("soups")).Id},
            new Dish { Id = 8, Name="Греческий салат",
                    Description="Со свежими овощами и фетой",
                    Price=150,
                    Weight=180, Image="img/dishes/Греческий.webp",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("salads")).Id},

            new Dish { Id = 9, Name="Креветки в кляре",
                    Description="С соусом тартар",
                    Price=25,
                    Weight=220, Image="img/dishes/Креветки.jpeg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("starters")).Id},

            new Dish { Id = 10, Name="Том Ям",
                    Description="Острый тайский суп с кокосовым молоком",
                    Price=30,
                    Weight=280, Image="img/dishes/ТомЯм.webp",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("soups")).Id},

            new Dish { Id = 11, Name="Салат с тунцом",
                    Description="Свежий салат с консервированным тунцом и овощами",
                    Price=20,
                    Weight=190, Image="img/dishes/СалатСТунцом.jpg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("salads")).Id},

            new Dish { Id = 12, Name="Мини-бургеры",
                    Description="Небольшие бургеры с говяжьей котлетой и сыром",
                    Price=23,
                    Weight=150, Image="img/dishes/МиниБургеры.jpeg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("starters")).Id},

            new Dish { Id = 13, Name="Мисо-суп",
                    Description="Японский суп с тофу и водорослями",
                    Price=15,
                    Weight=250, Image="img/dishes/МисоСуп.webp",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("soups")).Id},
            new Dish {Id =14 , Name = "Пина Колада",
                    Description="Безалкогольный коктейль с ананасовым соком и кокосовым молоком",
                    Price=13,
                    Weight=200, Image="img/dishes/ПинаКолада.jpg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("drinks")).Id},
            new Dish {Id =15 , Name = "Мохито",
                    Description="Безалкогольный освежающий коктейль с мятой и лаймом",
                    Price=13,
                    Weight=200, Image="img/dishes/Мохито.webp",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("drinks")).Id},
            new Dish {Id =16 , Name = "Чизкейк",
                    Description="Классический чизкейк с клубничным соусом",
                    Price=15,
                    Weight=150, Image="img/dishes/Чизкейк.webp",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("desserts")).Id},
            new Dish {Id =17 , Name = "Тирамису",
                    Description="Итальянский десерт с маскарпоне и кофе",
                       Price=16,
                    Weight=150, Image="img/dishes/Тирамису.jpg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("desserts")).Id},
            new Dish {Id = 18 , Name = "Крем-суп из тыквы",
                    Description="Сливочный суп с тыквой и специями",
                       Price=13,
                    Weight=300, Image="img/dishes/КремСупИзТыквы.jpg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("soups")).Id},
            new Dish {Id =19 , Name = "Лимонад",
                    Description="Домашний лимонад с мятой и лимоном",
                       Price=11,
                    Weight=250, Image="img/dishes/Лимонад.jpeg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("drinks")).Id},
            new Dish {Id=20, Name="Наполеон",
                    Description="Слоеный торт с заварным кремом",
                       Price=13,
                    Weight=180, Image="img/dishes/Наполеон.jpeg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("desserts")).Id},

            new Dish{Id=21, Name="Айс-ти",
                    Description="Освежающий холодный чай с лимоном",
                       Price=10,
                    Weight=250, Image="img/dishes/АйсТи.jpg",
                    CategoryId=_categories.Find(c=>c.NormalizedName.Equals("drinks")).Id},

            };
                }
                public Task<ResponseData<ProductListModel<Dish>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
                {
                      // Создать объект результата
                        var result = new ResponseData<ProductListModel<Dish>>();
                        // Id категории для фильрации
                        int? categoryId = null;
                        // если требуется фильтрация, то найти Id категории
                        // с заданным categoryNormalizedName
                        if (categoryNormalizedName != null)
                                categoryId = _categories
                                .Find(c =>
                                c.NormalizedName.Equals(categoryNormalizedName))
                                ?.Id;
                        // Выбрать объекты, отфильтрованные по Id категории,
                        // если этот Id имеется
                        var data = _dishes
                        .Where(d => categoryId == null ||
                        d.CategoryId.Equals(categoryId))?
                        .ToList();
                        // поместить ранные в объект результата
                        result.Data = new ProductListModel<Dish>() { Items = data };
                        // Если список пустой
                        if (data.Count == 0)
                        {
                                result.Success = false;
                                result.ErrorMessage = "Нет объектов в выбраннной категории";
                        }
                        // Вернуть результат
                        return Task.FromResult(result);
                }
                // public Task<ResponseData<List<Dish>>> GetProductListAsync(string? search, int categoryId)
                // {
                //     var query = _dishes.AsQueryable();

                //     if (!string.IsNullOrWhiteSpace(search))
                //         query = query.Where(d => d.Name.Contains(search, StringComparison.OrdinalIgnoreCase));

                //     if (categoryId > 0)
                //         query = query.Where(d => d.CategoryId == categoryId);

                //     return Task.FromResult(new ResponseData<List<Dish>>
                //     {
                //         Data = query.ToList()
                //     });
                // }

                public Task<ResponseData<Dish>> GetProductByIdAsync(int id)
                {
                        var dish = _dishes.FirstOrDefault(d => d.Id == id);


                        return Task.FromResult(new ResponseData<Dish>
                        {
                                Data = dish,
                                Success = dish != null,
                                ErrorMessage = dish == null ? "Блюдо не найдено" : null
                        });
                }

                public Task<ResponseData<Dish>> CreateProductAsync(Dish dish, IFormFile? file)
                {
                        dish.Id = _dishes.Max(d => d.Id) + 1;
                        _dishes.Add(dish);

                        return Task.FromResult(new ResponseData<Dish>
                        {
                                Data = dish
                        });
                }

                public Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish dish, IFormFile? file)
                {
                        var existing = _dishes.FirstOrDefault(d => d.Id == id);

                        if (existing == null)
                        {
                                return Task.FromResult(new ResponseData<Dish>
                                {
                                        Success = false,
                                        ErrorMessage = "Блюдо не найдено"
                                });
                        }

                        existing.Name = dish.Name;
                        existing.Description = dish.Description;
                        existing.Weight = dish.Weight;
                        existing.Image = dish.Image;
                        existing.CategoryId = dish.CategoryId;

                        return Task.FromResult(new ResponseData<Dish>
                        {
                                Data = existing
                        });
                }

                public Task<ResponseData<bool>> DeleteProductAsync(int id)
                {
                        var dish = _dishes.FirstOrDefault(d => d.Id == id);

                        if (dish != null)
                                _dishes.Remove(dish);

                        return Task.FromResult(new ResponseData<bool>
                        {
                                Data = dish != null,
                                Success = dish != null,
                                ErrorMessage = dish == null ? "Блюдо не найдено" : null
                        });
                }
        }
}