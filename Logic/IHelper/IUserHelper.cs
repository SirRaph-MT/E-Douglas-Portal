using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Logic.IHelper
{
    public interface IUserHelper
    {
        Task<ApplicationUser?> FindByEmailAsync(string email);
        string GetRoleLayout();
        IQueryable<ApplicationUserViewModel> GetUsers();
        string GetValidatedUrl(List<string> roles);
        Task<ApplicationUser?> RegisterUser(ApplicationUserViewModel applicationUserViewModel);
        IPagedList<ApplicationUserViewModel> Users(IPageListModel<ApplicationUserViewModel> model, int page);
    }
}
