using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string NormalizedName { get; set; }

        public string? Description { get; set; }

        /// Навигационное свойство - список блюд в этой категории
        public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
    }
}