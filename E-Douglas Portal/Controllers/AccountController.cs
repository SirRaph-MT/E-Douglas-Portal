using Core.DbContext;
using Core.Models;
using Core.ViewModels;
using E_Douglas_Portal.Models;
using Logic.IHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace E_Douglas_Portal.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailTemplateService _emailTemplateService;


        public AccountController
        (IUserHelper userHelper,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IEmailTemplateService emailTemplateService
        )
        {
            _userHelper = userHelper;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailTemplateService = emailTemplateService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Login(string email, string password)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return ResponseHelper.JsonError("Fill form correctly");
            }
            var user = await _userHelper.FindByEmailAsync(email).ConfigureAwait(false);
            if (user == null)
            {
                return ResponseHelper.JsonError("Invalid detail or account does not exist, contact your Admin");
            }
            var result = await _signInManager.PasswordSignInAsync(user, password, true, lockoutOnFailure: false).ConfigureAwait(false);

            if (!result.Succeeded)
            {
                return ResponseHelper.JsonError("Invalid user name or password");
            }
            user.Roles = (List<string?>)await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            user.UserRole = user.Roles.Contains(Constants.SuperAdminRole) ? Constants.SuperAdminRole :
                            user.Roles.Contains(Constants.AdminRole) ? Constants.AdminRole :
                            Constants.UserRole;
            var url = _userHelper.GetValidatedUrl(user.Roles);
            var currentUser = JsonConvert.SerializeObject(user, settings);
            HttpContext.Session.SetString("loggedInUser", currentUser);
            return ResponseHelper.JsonSuccessWithReturnUrl(url);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> Register(string userData)
        {
            if (string.IsNullOrEmpty(userData))
            {
                return ResponseHelper.ErrorMsg();
            }
            var applicationUser = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userData);
            if (applicationUser == null)
            {
                return ResponseHelper.ErrorMsg();
            }
            var user = await _userHelper.FindByEmailAsync(applicationUser.Email).ConfigureAwait(false);
            if (user != null)
            {
                return ResponseHelper.JsonError("Email already exists, please use another email");
            }
            var createStaff = await _userHelper.RegisterUser(applicationUser).ConfigureAwait(false);
            if (createStaff == null)
            {
                return ResponseHelper.JsonError("Unable to add");
            }
            var request = HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}";

            _emailTemplateService.SendRegistrationEmail(createStaff, baseUrl);
            return ResponseHelper.JsonSuccess("Registered successfully");
        }
    }
}
