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
