using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace E_Douglas_Portal.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
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
