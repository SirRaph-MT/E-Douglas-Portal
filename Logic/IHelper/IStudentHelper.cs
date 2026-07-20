using Core.ViewModels;
using X.PagedList;

namespace Logic.IHelper
{
    public interface IStudentHelper
    {
        IQueryable<ApplicationUserViewModel> GetAllStudents();
        IPagedList<ApplicationUserViewModel> Students(IPageListModel<ApplicationUserViewModel> model, int page);
        StudentDetailsViewModel? GetStudentDetails(string userId);
    }
}
