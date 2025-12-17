using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string NormalizedName { get; set; }

        public string? Description { get; set; }

        /// Навигационное свойство - список блюд в этой категории
        // public ICollection<Dish> Dishes { get; set; } = new List<Dish>();
        [JsonIgnore]
        public  ICollection<Dish> ?Dishes { get; set; } //Для предотвращения циклической сериализации объектов
// в модели Category навигационное свойство Dishes
// было помечено атрибутом [JsonIgnore].
// Это позволило передавать объект категории внутри блюда
// без возникновения зацикливания при формировании JSON.
    }
}