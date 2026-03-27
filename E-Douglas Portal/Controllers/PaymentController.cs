using Microsoft.AspNetCore.Mvc;

namespace E_Douglas_Portal.Controllers
{
    public class PaymentController : Controller
    {
        [HttpGet]
        public IActionResult Checkout(int enrollmentId)
        {
            return View();
        }

        [HttpGet]
        public IActionResult PaymentSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PaymentFailed()
        {
            return View();
        }

        [HttpPost]
        public JsonResult InitializePayment(object paymentData) => Json(new { });
        [HttpPost]
        public JsonResult VerifyPayment(string reference) => Json(new { });
    }
}
