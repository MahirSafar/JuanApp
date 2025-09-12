using JuanApp.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace JuanApp.Persistance.DAL.Seed
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            var roles = new[] { "Admin", "Member" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@gmail.com";
            var adminPassword = "Admin@123";

            var userExist = await userManager.FindByEmailAsync(adminEmail);
            
            if (userExist == null)
            {
                var adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    FullName = "Admin",
                    PhoneNumber = "0712345678",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                else
                    throw new Exception("Failed to create the admin user: " + string.Join(", ", result.Errors));
            }
        }
    }
}
