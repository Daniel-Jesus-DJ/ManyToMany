using ManyToMany.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace ManyToMany.Service
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            // Берем менеджеры пользователей и ролей
            var userManager = service.GetService<UserManager<Person>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            // 1. Создаем роль "Admin", если её нет
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // 2. Создаем пользователя-админа
            var adminEmail = "admin@game.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var newAdmin = new Person
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Admin",
                    FirstName = "Super",
                    EmailConfirmed = true,
                    Alter = new DateOnly(1990, 1, 1),
                    Geschlecht = Geschlecht.Männlich
                };

                
                var result = await userManager.CreateAsync(newAdmin, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
            else
            {
                //if adminUser lost adminRights
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}

