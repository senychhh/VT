using Microsoft.AspNetCore.Mvc;
using Kolbasin_lab1.Extensions;
using Domain.Models;

namespace Kolbasin_lab1.ViewComponents 
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() 
        {
            var cart = HttpContext.Session.Get<Cart>("cart") ?? new Cart();
            return View(cart);
        }
    }
}