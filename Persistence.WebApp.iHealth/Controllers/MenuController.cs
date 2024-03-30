using iHealth.Core.Application.Helpers;
using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.ViewModels.UserViewModels;
using iHealth.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace iHealth.Controllers
{
    public class MenuController : Controller
    {
        private readonly IUserService userService;
        private readonly ValidateUserSession validateUserSession;

        public MenuController(IUserService service, ValidateUserSession validateUser)
        {
            userService = service;
            validateUserSession = validateUser;
        }

        public IActionResult Index()
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            vm.Password = PasswordEncryption.ComputeSha256Hash(vm.Password);
            var Find = await userService.Authenticate(vm);
            if (Find.Success)
            {
                HttpContext.Session.Set<UserViewModel>("user", Find.User);
                return RedirectToAction("Index", "Menu");
            }
            else
                ViewBag.Failed = "Login Failed";
                return View(vm);
        }

        public IActionResult Logout()
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
        }
    }
}
