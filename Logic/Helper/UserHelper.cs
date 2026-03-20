using Core.DB;
using Core.Models;
using Core.Seed;
using Core.ViewModels;
using Logic.IHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Core.DbContext;
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
            foreach (var role in roles)
            {
                if (role == Constants.SuperAdminRole)
                    return Constants.SuperAdminDashboard;

                if (role == Constants.AdminRole)
                    return Constants.AdminDashboard;

                if (role == Constants.UserRole)
                    return Constants.UserDashboard;
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
                    // create a StudentProfile linked to the newly created user
                    var profile = new StudentProfile
                    {
                        UserId = user.Id,
                        DateOfBirth = applicationUserViewModel.DateOfBirth,
                        // other fields will be filled when the user completes their profile
                    };

                    await _context.StudentProfiles.AddAsync(profile).ConfigureAwait(false);
                    await _context.SaveChangesAsync().ConfigureAwait(false);

                    return user;
                }
            }
            return null;
        }
    }
}
