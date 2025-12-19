using Domain.Entities;

namespace Domain.Models
{
    public class CartItem
    {
        public Dish Item { get; set; } = null!;
        public int Qty { get; set; }
    }
}

