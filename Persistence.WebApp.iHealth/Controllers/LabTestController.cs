using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.ViewModels.LabTestViewModel;
using iHealth.Core.Application.ViewModels.PatientViewModels;
using iHealth.Middleware;
using Microsoft.AspNetCore.Mvc;
using Persistence.WebApp.iHealth.Controllers;

namespace iHealth.Controllers
{
    public class LabTestController : Controller
    {
        private readonly ILogger<LabTestController> _logger;
        private readonly ILabTestService labTestService;
        private readonly ValidateUserSession validateUserSession;

        public LabTestController(ILogger<LabTestController> logger, ILabTestService service, ValidateUserSession validateUserSession)
        {
            _logger = logger;
            labTestService = service;
            this.validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var list = await labTestService.GetAll();
            list = list.FindAll(e => e.Deleted == false);
            return View(list);
        }

        public async Task<IActionResult> AddLabTest()
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddLabTest(SaveLabTestViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var labTest = await labTestService.Add(vm);
            return RedirectToRoute(new { Action = "Index", Controller = "LabTest" });
        }

        public async Task<IActionResult> DeleteLabTest(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var vm = await labTestService.GetById(Id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLabTest(SaveLabTestViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await labTestService.Delete(vm.Id);
            return RedirectToRoute(new { Action = "Index", Controller = "LabTest" });
        }

        public async Task<IActionResult> UpdateLabTest(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var vm = await labTestService.GetById(Id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLabTest(SaveLabTestViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await labTestService.Update(vm);
            return RedirectToRoute(new { Action = "Index", Controller = "LabTest" });
        }

        public async Task<IActionResult> Details(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var list = await labTestService.GetAllWithIncludeAsync(new List<string>() { "LabResults" });
            var element = list.Find(e => e.Id == Id);
            return View(await labTestService.VMToSave(element));
        }
    }
}
