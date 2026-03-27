using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace E_Douglas_Portal.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CourseController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(object courseData)
        {
            return Json(new { isError = false, msg = "Course created successfully" });
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public JsonResult Edit(object courseData)
        {
            return Json(new { isError = false, msg = "Course updated successfully" });
        }

       
        [HttpPost]
        public JsonResult Delete(int id)
        {
            return Json(new { isError = false, msg = "Course deleted successfully" });
        }
    }
}
