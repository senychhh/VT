using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace Kolbasin_lab1.Controllers
{
    [Route("Image")] // все методы в этом контроллере доступны по маршруту /Image/


    public class ImageProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public ImageProfileController(UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _userManager = userManager;
            _environment = environment;
        }

       [HttpGet("GetAvatar")]
    public async Task<IActionResult> GetAvatar()
    {
        // Проверка: пользователь аутентифицирован
        if (!User.Identity?.IsAuthenticated ?? true)
        {
            return GetDefaultAvatar();
        }

        var email = User.Identity.Name;
        if (string.IsNullOrEmpty(email))
        {
            return GetDefaultAvatar();
        }

        var user = await _userManager.FindByEmailAsync(email);

        if (user == null || user.AvatarImage == null || user.AvatarImage.Length == 0)
        {
            return GetDefaultAvatar();
        }
    
        // Получение MIME-типа из базы данных
        var contentType = user.AvatarContentType ?? "image/png";

        return File(user.AvatarImage, contentType);
    }

    private IActionResult GetDefaultAvatar()
    {
        var defaultAvatarPath = Path.Combine(_environment.WebRootPath, "img", "profile.png");
        if (System.IO.File.Exists(defaultAvatarPath))
        {
            return PhysicalFile(defaultAvatarPath, "image/png");
        }
        // Если файл не найден, возвращаем пустое изображение
        return NotFound();
    }
        }
}