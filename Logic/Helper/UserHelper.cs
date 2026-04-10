using Core.DB;
using Core.DbContext;
using Core.Models;
using Core.Seed;
using Core.ViewModels;
using Logic.IHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.Extensions;
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

        public string GetRoleLayout()
        {
            var user = Utility.GetCurrentUser();
            if (user == null)
            {
                return Constants.DefaultLayout;
            }
            var isSuperAdmin = user.Roles.Contains(Constants.SuperAdminRole);
            return isSuperAdmin ? Constants.SuperAdminLayout : Constants.GeneralLayout;
        }

        public IPagedList<ApplicationUserViewModel> Users(IPageListModel<ApplicationUserViewModel> model, int page)
        {
            try
            {
                var query = GetUsers();

                if (!string.IsNullOrEmpty(model.Keyword))
                {
                    var key = model.Keyword.ToLower();

                    query = query.Where(x =>
                        x.FirstName.ToLower().Contains(key) ||
                        x.LastName.ToLower().Contains(key) ||
                        x.Email.ToLower().Contains(key) ||
                        x.PhoneNumber.ToLower().Contains(key) ||
                        x.FullName.ToLower().Contains(key));
                }

                if (model.StartDate.HasValue)
                {
                    query = query.Where(x => x.DateRegistered >= model.StartDate);
                }
                if (model.EndDate.HasValue)
                {
                    query = query.Where(x => x.DateOfBirth <= model.EndDate);
                }

                var logs = query
                    .OrderByDescending(x => x.DateRegistered)
                    .Select(r => new ApplicationUserViewModel
                    {
                        Id = r.Id,
                        FirstName = r.FirstName,
                        LastName = r.LastName,
                        Email = r.Email,
                        FullName = $"{r.FirstName} {r.LastName}",
                        PhoneNumber = r.PhoneNumber,
                        DateRegistered = r.DateRegistered,
                        DateOfBirth = r.DateOfBirth,
                        IsAdmin = r.IsAdmin
                    }).ToPagedList(page, 25);
                model.CanFilterByDeliveryStatus = true;

                return logs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public IQueryable<ApplicationUserViewModel> GetUsers()
        {
            var adminRoleId = _context.Roles
                .Where(r => r.Name == SeedItems.AdminRole)
                .Select(r => r.Id)
                .FirstOrDefault();

            var userRoleId = _context.Roles
                .Where(r => r.Name == SeedItems.UserRole)
                .Select(r => r.Id)
                .FirstOrDefault();

            var query =
                from u in _context.applicationUsers
                where !u.Deleted
                let isAdmin = _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && ur.RoleId == adminRoleId)
                let isUser = _context.UserRoles
                    .Any(ur => ur.UserId == u.Id && ur.RoleId == userRoleId)
                where isAdmin || isUser
                select new ApplicationUserViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    FullName = u.FirstName + " " + u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    DateRegistered = u.DateCreated,
                    DateOfBirth = u.DateOfBirth,
                    IsAdmin = isAdmin
                };

            return query;
        }
    }
}
