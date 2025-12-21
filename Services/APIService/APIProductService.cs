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
            var dish = await result.Content.ReadFromJsonAsync<Dish>();
            return new ResponseData<Dish>
            {
                Success = true,
                Data = dish
            };
        }

        return new ResponseData<Dish>
        {
            Success = false,
            ErrorMessage = result.StatusCode == System.Net.HttpStatusCode.NotFound 
                ? "Блюдо не найдено" 
                : "Ошибка получения продукта"
        };
    }

    public async Task<ResponseData<Dish>> CreateProductAsync(Dish dish, IFormFile? image)
    {
        try
        {
            // Сначала создаем блюдо
            var createResponse = await httpClient.PostAsJsonAsync(httpClient.BaseAddress, dish);
            
            if (!createResponse.IsSuccessStatusCode)
            {
                return new ResponseData<Dish>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка создания блюда: {createResponse.StatusCode}"
                };
            }

            var createdDish = await createResponse.Content.ReadFromJsonAsync<Dish>();
            
            // Если есть изображение, загружаем его
            if (image != null && createdDish != null)
            {
                using var content = new MultipartFormDataContent();
                using var stream = image.OpenReadStream();
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
                content.Add(streamContent, "image", image.FileName);

                var imageResponse = await httpClient.PostAsync($"{httpClient.BaseAddress}{createdDish.Id}", content);
                
                if (imageResponse.IsSuccessStatusCode)
                {
                    var imageUrl = await imageResponse.Content.ReadAsStringAsync();
                    createdDish.Image = imageUrl.Trim('"'); // Убираем кавычки из JSON строки
                }
            }

            return new ResponseData<Dish>
            {
                Success = true,
                Data = createdDish
            };
        }
        catch (Exception ex)
        {
            return new ResponseData<Dish>
            {
                Success = false,
                ErrorMessage = $"Ошибка при создании блюда: {ex.Message}"
            };
        }
    }

    public async Task<ResponseData<Dish>> UpdateProductAsync(int id, Dish dish, IFormFile? image)
    {
        try
        {
            var updateResponse = await httpClient.PutAsJsonAsync($"{httpClient.BaseAddress}{id}", dish);
            
            if (!updateResponse.IsSuccessStatusCode)
            {
                return new ResponseData<Dish>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка обновления блюда: {updateResponse.StatusCode}"
                };
            }

            // Если есть изображение, загружаем его
            if (image != null)
            {
                using var content = new MultipartFormDataContent();
                using var stream = image.OpenReadStream();
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
                content.Add(streamContent, "image", image.FileName);

                await httpClient.PostAsync($"{httpClient.BaseAddress}{id}", content);
            }

            // Получаем обновленное блюдо
            var getResponse = await httpClient.GetAsync($"{httpClient.BaseAddress}{id}");
            if (getResponse.IsSuccessStatusCode)
            {
                var updatedDish = await getResponse.Content.ReadFromJsonAsync<Dish>();
                return new ResponseData<Dish>
                {
                    Success = true,
                    Data = updatedDish
                };
            }

            return new ResponseData<Dish>
            {
                Success = true,
                Data = dish
            };
        }
        catch (Exception ex)
        {
            return new ResponseData<Dish>
            {
                Success = false,
                ErrorMessage = $"Ошибка при обновлении блюда: {ex.Message}"
            };
        }
    }

    public async Task<ResponseData<bool>> DeleteProductAsync(int id)
    {
        try
        {
            var result = await httpClient.DeleteAsync($"{httpClient.BaseAddress}{id}");

            if (result.IsSuccessStatusCode)
            {
                return new ResponseData<bool>
                {
                    Success = true,
                    Data = true
                };
            }

            return new ResponseData<bool>
            {
                Success = false,
                Data = false,
                ErrorMessage = result.StatusCode == System.Net.HttpStatusCode.NotFound 
                    ? "Блюдо не найдено" 
                    : $"Ошибка удаления: {result.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            return new ResponseData<bool>
            {
                Success = false,
                Data = false,
                ErrorMessage = $"Ошибка при удалении блюда: {ex.Message}"
            };
        }
    }

    public async Task<ResponseData<ProductListModel<Dish>>> GetDeletedProductsAsync(int pageNo = 1)
    {
        var baseUri = httpClient.BaseAddress?.ToString().TrimEnd('/') ?? "";
        var uri = $"{baseUri}/deleted";
        
        var queryData = new Dictionary<string, string>
        {
            ["pageNo"] = pageNo.ToString()
        };

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

    public async Task<ResponseData<bool>> RestoreProductAsync(int id)
    {
        try
        {
            var result = await httpClient.PostAsync($"{httpClient.BaseAddress}{id}/restore", null);

            if (result.IsSuccessStatusCode)
            {
                return new ResponseData<bool>
                {
                    Success = true,
                    Data = true
                };
            }

            return new ResponseData<bool>
            {
                Success = false,
                Data = false,
                ErrorMessage = result.StatusCode == System.Net.HttpStatusCode.NotFound 
                    ? "Блюдо не найдено" 
                    : $"Ошибка восстановления: {result.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            return new ResponseData<bool>
            {
                Success = false,
                Data = false,
                ErrorMessage = $"Ошибка при восстановлении блюда: {ex.Message}"
            };
        }
    }
    
}


//Интерфейс IProductService содержит методы для CRUD-операций.
// Все методы реализованы для работы с API через HTTP клиент.
