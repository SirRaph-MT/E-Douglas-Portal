using Core.Models;
using E_Douglas_Portal.Models;
using Logic.IHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Douglas_Portal.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IEnrollmentHelper _enrollmentHelper;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentController(IEnrollmentHelper enrollmentHelper, UserManager<ApplicationUser> userManager)
        {
            _enrollmentHelper = enrollmentHelper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout(long enrollmentId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge();

            var details = _enrollmentHelper.GetCheckoutDetails(enrollmentId, user.Id);
            if (details == null) return NotFound();

            return View(details);
        }

        [HttpGet]
        public IActionResult PaymentSuccess() => View();

        [HttpGet]
        public IActionResult PaymentFailed() => View();

        [HttpPost]
        public JsonResult InitializePayment(long enrollmentId)
        {
            // No Paystack/Flutterwave credentials configured — do not fake a success redirect.
            return ResponseHelper.JsonError(
                "Online payment isn't set up yet. Please contact the academy to arrange payment; " +
                "an admin will verify and activate your enrollment once received.");
        }

        [HttpPost]
        public JsonResult VerifyPayment(string reference) => Json(new { });
    }
}