using Microsoft.AspNetCore.Identity;

namespace VulnerableClientAdminUI.Extensions;

public static class SeedAdmin
{
    public static async Task SeedAdminUserAndRoleAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        const string adminRole = RoleNames.AdminRoleName;
        const string superUserRole = RoleNames.SuperUserRoleName;
        const string userRole = RoleNames.UserRoleName;

        const string adminEmail = "systemadmin@vca.local";
        const string adminPassword = "Admin123!";

        // Ensure roles exist
        foreach (var role in new[] { adminRole, superUserRole, userRole })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Ensure admin user exists
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                FirstName = "System",
                LastName = "Administrator",
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true,
                TwoFactorEnabled = false,
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (!result.Succeeded)
            {
                throw new Exception("Failed to create admin user: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        // Ensure admin user is in the admin role only
        if (!await userManager.IsInRoleAsync(adminUser, PolicyNames.AdminRolePolicy))
        {
            await userManager.AddToRoleAsync(adminUser, PolicyNames.AdminRolePolicy);
        }
    }
}