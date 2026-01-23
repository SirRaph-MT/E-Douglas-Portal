using Core.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.Seed
{
    public static class SeedItems
    {
        public const string SuperAdminRole = "SuperAdmin";
        public const string AdminRole = "Admin";
        public const string UserRole = "User";

        public static IList<IdentityRole> DefaultRoles() => new List<IdentityRole>
        {
            new IdentityRole(SuperAdminRole),
            new IdentityRole(AdminRole),
            new IdentityRole(UserRole)
        };

        public static IList<SeedUser> DefaultUsers() => new List<SeedUser>
        {
            new SeedUser
            {
                Email = "superadmin@edouglas.com",
                FirstName = "Super",
                LastName = "Admin",
                PhoneNumber = "0000000000",
                UserRole = SuperAdminRole
            },
            new SeedUser
            {
                Email = "admin@edouglas.com",
                FirstName = "System",
                LastName = "Admin",
                PhoneNumber = "0000000000",
                UserRole = AdminRole
            },
            new SeedUser
            {
                Email = "user@edouglas.com",
                FirstName = "System",
                LastName = "User",
                PhoneNumber = "0000000000",
                UserRole = UserRole
            }
        };
    }
}

