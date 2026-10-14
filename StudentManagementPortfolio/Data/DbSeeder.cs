using Microsoft.AspNetCore.Identity;
using StudentManagementPortfolio.Models;

namespace StudentManagementPortfolio.Data;

public static class DbSeeder
{
    public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // Ensure roles exist
        string[] roles = [AppRoles.Admin, AppRoles.Viewer];
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Optional configuration-based seeding (via user-secrets, appsettings, or environment variables)
        var adminEmail = configuration["InitialAdmin:Email"] ?? configuration["AdminUser:Email"];
        var adminPassword = configuration["InitialAdmin:Password"] ?? configuration["AdminUser:Password"];

        if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
        {
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);
            if (existingAdmin is null)
            {
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, AppRoles.Admin);
                }
            }
        }

        var viewerEmail = configuration["InitialViewer:Email"] ?? configuration["ViewerUser:Email"];
        var viewerPassword = configuration["InitialViewer:Password"] ?? configuration["ViewerUser:Password"];

        if (!string.IsNullOrWhiteSpace(viewerEmail) && !string.IsNullOrWhiteSpace(viewerPassword))
        {
            var existingViewer = await userManager.FindByEmailAsync(viewerEmail);
            if (existingViewer is null)
            {
                var viewerUser = new IdentityUser
                {
                    UserName = viewerEmail,
                    Email = viewerEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(viewerUser, viewerPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(viewerUser, AppRoles.Viewer);
                }
            }
        }
    }
}

