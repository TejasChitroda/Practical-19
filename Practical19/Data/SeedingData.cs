using Microsoft.AspNetCore.Identity;
using Practical19.Models;

namespace Practical19.Data
{
    public class SeedingData
    {
        public async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            string[] roleNames = { "Admin", "User" , "Manager" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Ensure an Admin user exists (Optional)
            var adminEmail = "admin123@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var newAdmin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Admin User"
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
            
            var managerEmail = "manager123@gmail.com";
            var managerUser = await userManager.FindByEmailAsync(managerEmail);
            if (managerUser == null)
            {
                var newManager = new User
                {
                    UserName = managerEmail,
                    Email = managerEmail,
                    FullName = "Manage User"
                };

                var result = await userManager.CreateAsync(newManager, "Manager@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newManager, "Manager");
                }
            }

            
        }

    }
}
