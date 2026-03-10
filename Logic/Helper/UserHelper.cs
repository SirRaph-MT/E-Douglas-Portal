using Core.DB;
using Core.Models;
using Core.Seed;
using Core.ViewModels;
using Logic.IHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
namespace Logic.Helper
{
    public class UserHelper: IUserHelper
    {
        private readonly AppDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserHelper
            (AppDBContext context, 
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<ApplicationUser?> FindByEmailAsync(string email)
        {
            return await _context.applicationUsers
                .Where(s => s.Email == email && !s.Deleted)
                .FirstOrDefaultAsync().ConfigureAwait(false);
        }
        public string GetValidatedUrl(List<string> roles)
        {
            var roleUrlMap = new Dictionary<string, string>
            {
                { SeedItems.SuperAdminRole, SeedItems.SuperAdminRole },
                { SeedItems.AdminRole, SeedItems.AdminRole },
                { SeedItems.UserRole, SeedItems.UserRole }
            };

            foreach (var role in roles)
            {
                if (roleUrlMap.TryGetValue(role, out var url))
                {
                    return url;
                }
            }

            return "/Account/Login";
        }

        public async Task<ApplicationUser?> RegisterUser(ApplicationUserViewModel applicationUserViewModel)
        {
            var user = new ApplicationUser
            {
                FirstName = applicationUserViewModel.FirstName,
                LastName = applicationUserViewModel.LastName,
                Email = applicationUserViewModel.Email,
                PhoneNumber = applicationUserViewModel.PhoneNumber,
                UserName = applicationUserViewModel.Email,
                DateOfBirth = applicationUserViewModel.DateOfBirth
            };

            var addedUser = await _userManager.CreateAsync(user, applicationUserViewModel.Password).ConfigureAwait(false);
            if (addedUser.Succeeded)
            {
                var addedUserToRole = await _userManager.AddToRoleAsync(user, SeedItems.UserRole).ConfigureAwait(false);
                if (addedUserToRole.Succeeded)
                {
                    return user;
                }
            }
            return null;
        }
    }
}
