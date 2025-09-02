// Create a new file, e.g., /Data/IdentityDataSeeder.cs

using Microsoft.AspNetCore.Identity;
using OpenArchival.DataAccess; // Your project's namespace for ApplicationUser

public static class IdentityDataSeeder
{
    public static async Task SeedRolesAndAdminUserAsync(IServiceProvider serviceProvider)
    {
        // Get the required services
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // --- Create Roles If They Don't Exist ---
        string[] roleNames = { "Admin", "User" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // --- Create a Default Admin User If It Doesn't Exist ---
        var adminEmail = "admin@admin.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true // Bypass email confirmation for the seeder
            };

            // Be sure to use a strong password from configuration in a real app
            var result = await userManager.CreateAsync(adminUser, "StrongAdminPassword123!");

            if (result.Succeeded)
            {
                // Assign the 'Admin' role to the new user
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}