using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Kolbasin_lab1.Controllers
{
    [Route("Image")] // все методы в этом контроллере доступны по маршруту /Image/....


    public class ImageProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ImageProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

       [HttpGet("GetAvatar")]
    public async Task<IActionResult> GetAvatar()
    {
        // Проверка: пользователь аутентифицирован
        if (!User.Identity?.IsAuthenticated ?? true)
        {
            return File("/img/profile.png", "image/png");
        }

        var email = User.Identity.Name;
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null || user.AvatarImage == null)
        {
            return File("/img/profile.png", "image/png");
        }
    
        // Получение MIME-типа из базы данных
        var contentType = user.AvatarContentType ?? "image/png";

        return File(user.AvatarImage, contentType);
    }
        }
}