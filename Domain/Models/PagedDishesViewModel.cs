using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Kolbasin_lab1.Models
{
    public class PagedDishesViewModel
    {
        public IEnumerable<Dish> Items { get; set; } = new List<Dish>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string? Category { get; set; }
    }
}