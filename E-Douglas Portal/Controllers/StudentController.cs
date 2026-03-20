using Microsoft.AspNetCore.Mvc;
using Core.DB;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Logic.IHelper;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace E_Douglas_Portal.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        // everything in this country will be reviewed later, just want to get the basic structure down, then we can review and make it better
        public StudentController(AppDBContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CompleteProfile()
        {
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user == null) return Challenge();

            var profile = await _context.StudentProfiles
                .FindAsync(user.Id);

            if (profile == null)
            {
                profile = new StudentProfile { UserId = user.Id };
                _context.StudentProfiles.Add(profile);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }

            return View(profile);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CompleteProfile(StudentProfile model, IFormFile? passportFile)
        {
            var user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user == null) return Json(new { isError = true, msg = "Unauthorized" });

            var profile = await _context.StudentProfiles.FindAsync(model.Id).ConfigureAwait(false);
            if (profile == null) return Json(new { isError = true, msg = "Profile not found" });

            profile.DateOfBirth = model.DateOfBirth;
            profile.SponsorName = model.SponsorName;
            profile.SponsorAddres = model.SponsorAddres;
            profile.SponsorPhone = model.SponsorPhone;
            profile.StudentType = model.StudentType;
            profile.ProfileCompletionStage = Core.Enums.E_DouglasEnums.ProfileCompletionStage.BioData;

            if (passportFile != null && passportFile.Length > 0)
            {
                var uploads = Path.Combine(_env.WebRootPath, "uploads", "passports");
                if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
                var fileName = user.Id + Path.GetExtension(passportFile.FileName);
                var filePath = Path.Combine(uploads, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await passportFile.CopyToAsync(stream).ConfigureAwait(false);
                }
                profile.PassportUrl = $"/uploads/passports/{fileName}";
                profile.ProfileCompletionStage = Core.Enums.E_DouglasEnums.ProfileCompletionStage.Documents;
            }

            _context.StudentProfiles.Update(profile);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return Json(new { isError = false, msg = "Profile saved", returnUrl = "/Student/Index" });
        }
    }
}
