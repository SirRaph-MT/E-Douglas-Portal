using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core.ViewModels;

namespace E_Douglas_Portal.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard()
        {
            var model = new AdminDashboardViewModel
            {
                UserName = User.Identity.Name ?? "Admin",
                TotalStudents = 248,
                ActiveCourses = 12,
                CertificatesIssued = 87,
                TotalEarnings = 4800000
            };

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
