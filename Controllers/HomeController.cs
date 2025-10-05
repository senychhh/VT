using Microsoft.AspNetCore.Mvc;

namespace Kolbasin_lab1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() 
        {
            return View();
        }
    }
}