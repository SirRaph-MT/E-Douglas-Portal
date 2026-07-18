using Core.DB;
using Core.ViewModels;
using Logic.IHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Douglas_Portal.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly ICourseHelper _courseHelper;
        private readonly AppDBContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IStudentHelper _students;

        public AdminController(ICourseHelper courseHelper, AppDBContext context, IUserHelper userHelper, IStudentHelper students)
        {
            _courseHelper = courseHelper;
            _context = context;
            _userHelper = userHelper;
            _students = students;
        }
        [HttpGet]
        public IActionResult Dashboard()
        {
            ViewBag.Layout = _userHelper.GetRoleLayout();
            var students = _students.GetAllStudents();
            //var data = new AdminDashboardViewModel
            //{
            //    UserName = Utility.GetCurrentUser().FullName,
            //    ProjectCount = projects.ToList().Count,
            //    ClientCount = _userHelper.GetUsers().ToList().Count,
            //    Projects = projects.ToList(),
            //    TotalEarnings = _context.Contributions.Sum(x => x.Amount),
            //    ContibutorsCount = _projectHelper.GetContributors().Count
            //};
            return View(students);
        }

        [HttpGet]
        public IActionResult AllUSers(IPageListModel<ApplicationUserViewModel> model, int page = 1)
        {
            //ViewBag.Layout = _userHelper.GetRoleLayout();
            var users = _userHelper.Users(model, page);
            model.Model = users;
            model.SearchAction = "AllUSers";
            model.SearchController = "Admin";
            return View(model);
        }

        [HttpGet]
        public IActionResult Students()
        {
            return View();
        }

        [HttpGet]
        public IActionResult StudentDetails(int id)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Enrollments()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Approvals()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ApproveCompletion(int studentCourseId)
        {
            return Json(new { isError = false, msg = "Certificate approved successfully" });
        }

        [HttpPost]
        public JsonResult RejectCompletion(int studentCourseId)
        {
            return Json(new { isError = false, msg = "Certificate request rejected" });
        }
    }
}
