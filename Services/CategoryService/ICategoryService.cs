using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Models;

namespace Kolbasin_lab1.Services
{
    public interface ICategoryService
    {
        /// Получение списка всех категорий
        public Task<ResponseData<List<Category>>> GetCategoryListAsync();
    }
}