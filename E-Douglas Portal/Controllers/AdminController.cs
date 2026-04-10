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

        public AdminController(ICourseHelper courseHelper, AppDBContext context, IUserHelper userHelper)
        {
            _courseHelper = courseHelper;
            _context = context;
            _userHelper = userHelper;
        }
        [HttpGet]
        public IActionResult Dashboard()
        {
            var model = new AdminDashboardViewModel
            {
                UserName = User.Identity.Name ?? "Admin",
                TotalStudents = 248,
                ActiveCourses = 12,
                CertificatesIssued = 87,
                TotalEarnings = 4800000,
                Users = _context.Users.Count(),
            };


            return View(model);
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
