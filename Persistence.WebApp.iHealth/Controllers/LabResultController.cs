using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.ViewModels.LabResultViewModel;
using iHealth.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace iHealth.Controllers
{
    public class LabResultController : Controller
    {
        private readonly ILogger<LabResultController> _logger;
        private readonly ILabResultService labResultService;
        private readonly IAppointmentService appointmentService;
        private readonly IPatientService patientService;
        private readonly IDoctorService doctorService;
        private readonly ValidateUserSession validateUserSession;

        public LabResultController(ILogger<LabResultController> logger, 
            ILabResultService service, 
            ValidateUserSession validateUserSession, 
            IAppointmentService appointmentService, 
            IPatientService patientService,
            IDoctorService doctorService)
        {
            _logger = logger;
            labResultService = service;
            this.validateUserSession = validateUserSession;
            this.appointmentService = appointmentService;
            this.patientService = patientService;
            this.doctorService = doctorService;
        }

        public async Task<IActionResult> Index(FilterViewModel filter = null)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            //Desde aquí aparece el HOLAA
            var list = await labResultService.GetAllWithFilter(filter, await patientService.GetAll()); 
            foreach(var item in list)
            {
                var appointments = await appointmentService.GetAllWithIncludeAsync(new List<string>() { "Patient", "Doctor" });
                item.Appointment = await appointmentService.VMToModel(appointments.Find(e => e.Id == item.AppointmentId));
            }
            FilterCompletedViewModel vm = new();
            vm.Filter = filter;
            vm.LabResults = list;
            vm.Patients = await patientService.GetAll();
            vm.Doctors = await doctorService.GetAll();
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddLabResult(SaveLabResultViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }            //Tengo que agregarle el subir fotos
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await labResultService.Add(vm);
            return RedirectToRoute(new { Action = "Index", Controller = "LabResult" });
        }

        //No se borrarán
        public async Task<IActionResult> DeleteLabResult(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var vm = await labResultService.GetById(Id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLabResult(SaveLabResultViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await labResultService.Delete(vm.Id);
            return RedirectToRoute(new { Action = "Index", Controller = "LabResult" });
        }

        public async Task<IActionResult> UpdateLabResult(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var vm = await labResultService.GetById(Id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLabResult(SaveLabResultViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (ModelState["Result"] == null)
            {
                return View(vm);
            }
            vm.Completed = true;
            await labResultService.Update(vm);
            return RedirectToRoute(new { Action = "Index", Controller = "LabResult" });
        }
    }
}
