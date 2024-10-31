using Identity.API.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.API.Extensions;

public static class IdentityDataInitializer
{
    public static async Task SeedData(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Seed roles
        await SeedRoles(roleManager);

        // Seed users
        await SeedUsers(userManager);
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin")) await roleManager.CreateAsync(new IdentityRole("Admin"));

        if (!await roleManager.RoleExistsAsync("Customer")) await roleManager.CreateAsync(new IdentityRole("Customer"));
    }

    private static async Task SeedUsers(UserManager<User> userManager)
    {
        // Seed Admin User
        if (userManager.Users.All(u => u.UserName != "admin@myapp.com"))
        {
            var adminUser = new User
            {
                UserName = "admin@myapp.com",
                Email = "admin@myapp.com"
            };
            var result = await userManager.CreateAsync(adminUser, "AdminPassword@123");
            if (result.Succeeded) await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        // Seed Customer Users
        var customerEmails = new[] { "customer1@myapp.com", "customer2@myapp.com" };
        foreach (var email in customerEmails)
            if (userManager.Users.All(u => u.UserName != email))
            {
                var customerUser = new User
                {
                    UserName = email,
                    Email = email
                };
                var result = await userManager.CreateAsync(customerUser, "CustomerPassword@123");
                if (result.Succeeded) await userManager.AddToRoleAsync(customerUser, "Customer");
            }
    }
}