using System.Net.Http.Json;
using Domain.Entities;
using Domain.Models;
using Kolbasin_lab1.Services;

public class ApiCategoryService(HttpClient httpClient)
    : ICategoryService
{
    public async Task<ResponseData<List<Category>>> GetCategoryListAsync()
    {
        try
        {
            // Получаем массив категорий напрямую
            var categories = await httpClient.GetFromJsonAsync<List<Category>>(httpClient.BaseAddress);

            return new ResponseData<List<Category>>
            {
                Success = true,
                Data = categories
            };
        }
        catch
        {
            return new ResponseData<List<Category>>
            {
                Success = false,
                ErrorMessage = "Ошибка чтения API"
            };
        }
    }
}

// В проекте XXX.UI сервисы MemoryProductService и MemoryCategoryService
// были заменены на ApiProductService и ApiCategoryService.
// Новые сервисы получают данные из REST API проекта XXX.API
// с использованием HttpClient.
// Регистрация сервисов выполнена в классе Program
// с использованием AddHttpClient.
// Это позволило отделить пользовательский интерфейс
// от источника данных и обеспечить работу UI с реальной базой данных.
// Получаем список напрямую через GetFromJsonAsync<List<Category>>().
// Потом оборачиваем его в ResponseData, чтобы интерфейс сервиса оставался прежним.
// То есть мы отдельно делаем:
// Десериализация JSON → List<Category>
// Оборачивание → ResponseData<List<Category>>
// Так UI получает данные в ожидаемом формате и ошибок десериализации больше нет.