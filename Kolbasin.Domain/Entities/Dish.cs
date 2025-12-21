using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
        public class Dish
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }


        public decimal Price { get; set; }

        public int? Weight { get; set; }


        /// Путь к файлу изображения блюда
        public string? Image { get; set; }

        public int CategoryId { get; set; }

        /// Навигационное свойство - категория блюда
        public Category ?Category { get; set; }

        /// Признак удаления (мягкое удаление)
        public bool IsDeleted { get; set; } = false;

        /// Дата и время удаления
        public DateTime? DeletedAt { get; set; }
    }
}