using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.ViewModels.PatientViewModels;
using iHealth.Core.Application.ViewModels.UserViewModels;
using iHealth.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace Persistence.WebApp.iHealth.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService userService;
        private readonly ValidateUserSession validateUserSession;

        public UserController(ILogger<UserController> logger, IUserService service, ValidateUserSession validateSession)
        {
            this._logger = logger;
            userService = service;
            this.validateUserSession = validateSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var list = await userService.GetAll();
            list = list.FindAll(e => e.Deleted == false);
            return View(list);
        }

        public async Task<IActionResult> AddUser()
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(SaveUserViewModel vm)
        {
            ViewBag.Existence = "";
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }            //Tengo que agregarle el subir fotos
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if(await userService.UsernameAlreadyExists(vm.UserName))
            {
                ViewBag.Existence = "This username is already registered";
                return View(vm);
            }
            await userService.Add(vm);
            return RedirectToRoute(new { Action = "Login", Controller = "Menu" });
        }

        public async Task<IActionResult> DeleteUser(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var vm = await userService.GetById(Id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(SavePatientViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            await userService.Delete(vm.Id);
            return RedirectToRoute(new { Action = "Index", Controller = "User" });
        }

        public async Task<IActionResult> UpdateUser(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }            //Las contraseñas deben ser opcionales
            var vm = await userService.GetById(Id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(SaveUserViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (!(ModelState.ErrorCount == 2 
                && (ModelState["Password"] == null 
                && ModelState["PasswordConfirmation"] == null)) 
                == false)
            {
                return View(vm);
            }
            if (await userService.UsernameAlreadyExists(vm.UserName))
            {
                ViewBag.Existence = "This username is already registered";
                return View(vm);
            }
            await userService.Update(vm);
            return RedirectToRoute(new { Action = "Index", Controller = "User" });
        }

        public async Task<IActionResult> Details(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var vm = await userService.GetById(Id);
            
            return View(await userService.SaveToVM(vm));
        }
    }
}