using Core.Models;
using E_Douglas_Portal.Models;
using Logic.IHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace E_Douglas_Portal.Controllers
{
    [Authorize]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentHelper _enrollmentHelper;
        private readonly UserManager<ApplicationUser> _userManager;

        public EnrollmentController(IEnrollmentHelper enrollmentHelper, UserManager<ApplicationUser> userManager)
        {
            _enrollmentHelper = enrollmentHelper;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<JsonResult> Enroll(long courseId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return ResponseHelper.JsonError("Unauthorized");

            var result = _enrollmentHelper.CreateEnrollment(user.Id, courseId);
            if (result.IsError) return ResponseHelper.JsonError(result.Message ?? "Unable to enroll");

            return ResponseHelper.JsonSuccessWithReturnUrl($"/Payment/Checkout/{result.EnrollmentId}");
        }
    }
}