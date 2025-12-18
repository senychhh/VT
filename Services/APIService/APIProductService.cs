using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Domain.Entities;
using Domain.Models;
using Kolbasin_lab1.Services;

public class ApiProductService(HttpClient httpClient)
    : IProductService
{
    public async Task<ResponseData<ProductListModel<Dish>>>
        GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var uri = httpClient.BaseAddress;

        var queryData = new Dictionary<string, string>
        {
            ["pageNo"] = pageNo.ToString()
        };

        if (!string.IsNullOrEmpty(categoryNormalizedName))
        {
            queryData.Add("category", categoryNormalizedName);
        }

        var query = QueryString.Create(queryData);

        var result = await httpClient.GetAsync(uri + query.Value);

        if (result.IsSuccessStatusCode)
        {
            return await result.Content
                .ReadFromJsonAsync<ResponseData<ProductListModel<Dish>>>();
        }

        return new ResponseData<ProductListModel<Dish>>
        {
            Success = false,
            ErrorMessage = "Ошибка чтения API"
        };
    }

    // ---------------- ДОПОЛНИТЕЛЬНЫЕ МЕТОДЫ ----------------

    public async Task<ResponseData<Dish>> GetProductByIdAsync(int id)
    {
        var result = await httpClient.GetAsync($"{httpClient.BaseAddress}{id}");

        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadFromJsonAsync<ResponseData<Dish>>();
        }

        return new ResponseData<Dish>
        {
            Success = false,
            ErrorMessage = "Ошибка получения продукта"
        };
    }

    public async Task<ResponseData<Dish>> CreateProductAsync(Dish dish, IFormFile? image)
    {
        return new ResponseData<Dish>
        {
            Success = false,
            ErrorMessage = "Метод не реализован в API-сервисе"
        };
    }

    public async Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish dish, IFormFile? image)
    {
        return new ResponseData<Dish>
        {
            Success = false,
            ErrorMessage = "Метод не реализован в API-сервисе"
        };
    }

    public async Task<ResponseData<bool>> DeleteProductAsync(int id)
    {
        return new ResponseData<bool>
        {
            Success = false,
            ErrorMessage = "Метод не реализован в API-сервисе"
        };
    }
    
}


//Интерфейс IProductService содержит методы для CRUD-операций.
// В рамках данной лабораторной работы используется только метод
// получения списка продуктов.
// Остальные методы реализованы в виде заглушек,
// так как соответствующие endpoints API не используются в Kolbasin_lab1.
