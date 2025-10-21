using Microsoft.AspNetCore.Mvc;

namespace Kolbasin_lab1.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}