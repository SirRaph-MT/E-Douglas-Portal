using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Logic.IHelper
{
    public interface ICourseHelper
    {
        IPagedList<CourseViewModel> Courses(IPageListModel<CourseViewModel> model, int page);
    }
}
