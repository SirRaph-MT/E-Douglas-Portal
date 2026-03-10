using Microsoft.AspNetCore.Mvc;

namespace E_Douglas_Portal.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
