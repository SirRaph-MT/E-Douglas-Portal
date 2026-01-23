using Core.Models;
using Core.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Core.Enums.E_DouglasEnums;

namespace Core.DB
{
    public static class CoreSeed
    {
        public static async Task SeedAsync(
            AppDBContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
            await SeedDropdownsAsync(context);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in SeedItems.DefaultRoles())
            {
                if (!await roleManager.RoleExistsAsync(role.Name!))
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            foreach (var seedUser in SeedItems.DefaultUsers())
            {
                var existingUser = await userManager.FindByEmailAsync(seedUser.Email);
                if (existingUser != null) continue;

                var user = new ApplicationUser
                {
                    UserName = seedUser.Email,
                    Email = seedUser.Email,
                    FirstName = seedUser.FirstName,
                    LastName = seedUser.LastName,
                    PhoneNumber = seedUser.PhoneNumber,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "11111");

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ",
                        result.Errors.Select(e => e.Description)));
                }

                await userManager.AddToRoleAsync(user, seedUser.UserRole);
            }
        }

        private static async Task SeedDropdownsAsync(AppDBContext context)
        {
            if (await context.DropDowns.AnyAsync()) return;

            context.DropDowns.AddRange(
                new DropDown { DropdownKey = DropdownEnums.Gender, Name = "Male" },
                new DropDown { DropdownKey = DropdownEnums.Gender, Name = "Female" },
                new DropDown { DropdownKey = DropdownEnums.Gender, Name = "Prefer not to say" }
            );

            await context.SaveChangesAsync();
        }
    }
}
