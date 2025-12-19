using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Kolbasin_lab1.Data
{
    public class DbInit
    {
        public static async Task SeedData(WebApplication application)
        {
            using var scope = application.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Список пользователей, которые должны иметь права администратора
            var allowedAdmins = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "admin@gmail.com"
                // можно добавить ещё: "другой@почта.com"
            };

            // 1) Создаём администратора, если его нет
            await EnsureAdminUser(userManager, "admin@gmail.com", "123456");

            // 2) Удаляем claim "admin" у всех остальных пользователей
            foreach (var u in userManager.Users)
            {
                var email = u.Email ?? "";
                if (allowedAdmins.Contains(email))
                    continue;

                var claims = await userManager.GetClaimsAsync(u);
                var adminClaims = claims.Where(c => c.Type == ClaimTypes.Role && c.Value == "admin").ToList();
                foreach (var c in adminClaims)
                    await userManager.RemoveClaimAsync(u, c);
            }
        }

        private static async Task EnsureAdminUser(UserManager<ApplicationUser> userManager, string email, string password)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    foreach (var err in result.Errors)
                        Console.WriteLine($"Error: {err.Description}");
                    return;
                }
            }

            await EnsureAdminClaim(userManager, user);
        }

        private static async Task EnsureAdminClaim(UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            var claims = await userManager.GetClaimsAsync(user);
            var hasAdmin = claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "admin");
            if (!hasAdmin)
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "admin"));
        }
    }
}

// using Microsoft.AspNetCore.Identity;
// using System.Security.Claims;

// namespace Kolbasin_lab1.Data
// {
//     public class DbInit
//     {
//         public static async Task SeedData(WebApplication app)
//         {
//             using var scope = app.Services.CreateScope();
//             var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

//             string adminEmail = "admin@gmail.com";
//             string adminPassword = "123456";

//             // 1️⃣ Проверяем, существует ли пользователь с таким email
//             var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
//             if (existingAdmin != null)
//             {
//                 // Удаляем старого администратора
//                 var deleteResult = await userManager.DeleteAsync(existingAdmin);
//                 if (!deleteResult.Succeeded)
//                 {
//                     foreach (var error in deleteResult.Errors)
//                         Console.WriteLine($"Ошибка при удалении старого админа: {error.Description}");
//                 }
//                 else
//                 {
//                     Console.WriteLine("Старый админ успешно удален");
//                 }
//             }

//             // 2️⃣ Создаём нового администратора
//             var admin = new ApplicationUser
//             {
//                 Email = adminEmail,
//                 UserName = adminEmail,
//                 EmailConfirmed = true
//             };

//             var createResult = await userManager.CreateAsync(admin, adminPassword);
//             if (!createResult.Succeeded)
//             {
//                 foreach (var error in createResult.Errors)
//                     Console.WriteLine($"Ошибка при создании нового админа: {error.Description}");
//                 return;
//             }

//             // 3️⃣ Добавляем claim Role=admin
//             var claim = new Claim(ClaimTypes.Role, "admin");
//             await userManager.AddClaimAsync(admin, claim);

//             Console.WriteLine("Новый админ создан и claim Role=admin добавлен");
//         }
//     }
// }
