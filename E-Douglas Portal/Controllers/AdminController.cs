using Core.DB;
using Core.ViewModels;
using Logic;                    
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
        private readonly IEnrollmentHelper _enrollmentHelper;

        public AdminController(ICourseHelper courseHelper, AppDBContext context, IUserHelper userHelper, IStudentHelper students, IEnrollmentHelper enrollmentHelper)
        {
            _courseHelper = courseHelper;
            _context = context;
            _userHelper = userHelper;
            _students = students;
            _enrollmentHelper = enrollmentHelper;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var currentUser = Utility.GetCurrentUser();

            var data = new AdminDashboardViewModel
            {
                UserName = currentUser?.FullName?.Trim() ?? "Admin",
                TotalStudents = _students.GetAllStudents().Count(),
                ActiveCourses = _context.Courses.Count(c => !c.Deleted && c.IsActive),
                CertificatesIssued = 0, // TODO: wire up once Approvals/Certificates module is built
                TotalEarnings = 0,      // TODO: wire up once Payment module is built
                Users = _userHelper.GetUsers().Count()
            };

            return View(data);
        }

        [HttpGet]
        public IActionResult AllUSers(IPageListModel<ApplicationUserViewModel> model, int page = 1)
        {
            var users = _userHelper.Users(model, page);
            model.Model = users;
            model.SearchAction = "AllUSers";
            model.SearchController = "Admin";
            return View(model);
        }

        [HttpGet]
        public IActionResult Students(IPageListModel<ApplicationUserViewModel> model, int page = 1)
        {
            var students = _students.Students(model, page);
            model.Model = students;
            model.SearchAction = "Students";
            model.SearchController = "Admin";
            return View(model);
        }

        [HttpGet]
        public IActionResult StudentDetails(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var details = _students.GetStudentDetails(id);
            if (details == null) return NotFound();

            return View(details);
        }

        [HttpGet]
        public IActionResult Enrollments(IPageListModel<EnrollmentListViewModel> model, int page = 1)
        {
            var enrollments = _enrollmentHelper.Enrollments(model, page);
            model.Model = enrollments;
            model.SearchAction = "Enrollments";
            model.SearchController = "Admin";
            return View(model);
        }

        [HttpGet]
        public IActionResult Approvals() => View();

        [HttpPost]
        public JsonResult ApproveCompletion(int studentCourseId) =>
            Json(new { isError = false, msg = "Certificate approved successfully" });

        [HttpPost]
        public JsonResult RejectCompletion(int studentCourseId) =>
            Json(new { isError = false, msg = "Certificate request rejected" });
    }
}
