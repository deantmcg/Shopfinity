using Microsoft.AspNetCore.Identity;
using Shopfinity.ProductService.Constants;
using Shopfinity.ProductService.Models;

public static class SeedUserData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        // Create roles based on the UserRole enum
        foreach (var roleName in UserRole.All)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        if (!userManager.Users.Any())
        {
            // Seed an Admin user
            var adminUser = new ApplicationUser
            {
                FirstName = "Admin",
                LastName = "User",
                UserName = "admin@shopfinity.com",
                Email = "admin@shopfinity.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, "Admin@123");
            await userManager.AddToRoleAsync(adminUser, UserRole.Admin);

            // Seed a Manager user
            var managerUser = new ApplicationUser
            {
                FirstName = "Manager",
                LastName = "User",
                UserName = "manager@shopfinity.com",
                Email = "manager@shopfinity.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(managerUser, "Manager@123");
            await userManager.AddToRoleAsync(managerUser, UserRole.Manager);

            // Seed a Customer user
            var customerUser = new ApplicationUser
            {
                FirstName = "Customer",
                LastName = "User",
                UserName = "customer@shopfinity.com",
                Email = "customer@shopfinity.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(customerUser, "Customer@123");
            await userManager.AddToRoleAsync(customerUser, UserRole.Customer);
        }
    }
}