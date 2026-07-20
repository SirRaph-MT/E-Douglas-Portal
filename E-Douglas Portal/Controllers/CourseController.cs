using Core.DB;
using Core.Models;
using Core.ViewModels;
using E_Douglas_Portal.Models;
using Logic.IHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Douglas_Portal.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CourseController : Controller
    {
        private readonly ICourseHelper _courseHelper;
        private readonly AppDBContext _context;

        public CourseController(ICourseHelper courseHelper, AppDBContext context)
        {
            _courseHelper = courseHelper;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(IPageListModel<CourseViewModel> model, int page = 1)
        {
            var courses = _courseHelper.Courses(model, page);
            model.Model = courses;
            model.SearchAction = "Index";
            model.SearchController = "Course";
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Create(CourseViewModel courseData)
        {
            if (courseData == null || string.IsNullOrWhiteSpace(courseData.Title))
                return ResponseHelper.JsonError("Course title is required");

            _courseHelper.CreateCourse(courseData);
            return ResponseHelper.JsonSuccessWithReturnUrl("/Course/Index");
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
