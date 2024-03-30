using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.ViewModels.AppointmentViewModel;
using iHealth.Core.Application.ViewModels.DoctorViewModels;
using iHealth.Core.Application.ViewModels.LabResultViewModel;
using iHealth.Core.Application.ViewModels.LabTestViewModel;
using iHealth.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace iHealth.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly IAppointmentService appointmentService;
        private readonly IDoctorService doctorService;
        private readonly IPatientService patientService;
        private readonly ValidateUserSession validateUserSession;
        private readonly ILabTestService labTestService;
        private readonly ILabResultService labResultService;
        public AppointmentController(
            ILogger<AppointmentController> logger, 
            IAppointmentService service, 
            ValidateUserSession validateUserSession, 
            IDoctorService doctorService, 
            IPatientService patientService,
            ILabTestService labTestService,
            ILabResultService labResultService)
        {
            this._logger = logger;
            appointmentService = service;
            this.validateUserSession = validateUserSession;
            this.patientService = patientService;
            this.doctorService = doctorService;
            this.labTestService = labTestService;
            this.labResultService = labResultService;
        }

        public async Task<IActionResult> Index()
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var list = await appointmentService.GetAllWithIncludeAsync(new List<string>() { "Doctor", "Patient", "LabResult"});
            list = list.FindAll(e => e.Deleted == false);
            return View(list);
        }

        public async Task<IActionResult> AddAppointment()
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }

            AddAppointmentViewModel addAVM = new();
            addAVM.Doctors = await doctorService.GetAll();
            addAVM.Patients = await patientService.GetAll();
            return View(addAVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddAppointment(AddAppointmentViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (vm.MedicalConcern == null)
            {
                vm.Doctors = await doctorService.GetAll();
                vm.Patients = await patientService.GetAll();
                return View(vm);
            }
            DateTime Date = new DateTime(vm.Date.Year, vm.Date.Month, vm.Date.Day, vm.Time.Hour, vm.Time.Minute, vm.Time.Second);
            SaveAppointmentViewModel SAVP = new();
            SAVP.DoctorId = vm.ChosenDoctorId;
            SAVP.PatientId = vm.ChosenPatientId;
            SAVP.AppointmentDate = Date;
            SAVP.Status = AppointmentStatus.PendingConsultation;
            SAVP.MedicalConcern = vm.MedicalConcern;
            await appointmentService.Add(SAVP);
            return RedirectToRoute(new { Action = "Index", Controller = "Appointment" });
        }

        public async Task<IActionResult> DeleteAppointment(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var list = await appointmentService.GetAllWithIncludeAsync(new List<string>());
            var vm = list.Find(e => e.Id == Id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(AppointmentViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            await appointmentService.Delete(vm.Id);
            return RedirectToRoute(new { Action = "Index", Controller = "Appointment" });
        }

        public async Task<IActionResult> UpdateAppointment(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var vm = await appointmentService.GetById(Id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAppointment(SaveAppointmentViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await appointmentService.Update(vm);
            return RedirectToRoute(new { Action = "Index", Controller = "Appointment" });
        }

        public async Task<IActionResult> Consulte(int Id)
        {
            //AQUIIIIIIIIIIIIIIIIIIIIIIIIIIIIIII
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var list = await appointmentService.GetAllWithIncludeAsync(new List<string>() { "Patient", "Doctor", "LabResult"});
            var appointment = list.Find(x => x.Id == Id);
            var LTA = await labTestService.GetAllWithIncludeAsync(new List<string>() { });
            ConsultateViewModel CVM = new();
            CVM.Appointment = appointment;
            CVM.LabTestsAvailable = LTA.Where(e=>e.Deleted != true).ToList();

            return View(CVM);
        }
        [HttpPost]
        public async Task<IActionResult> Consulte(ConsultateViewModel cvm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var list = await labTestService.GetAllWithIncludeAsync(new List<string>() { "LabResults"});
            list = list.Where(e => e.Deleted != true).ToList();
            var Appointment = await appointmentService.GetById(cvm.Appointment.Id);
            var LabTestAvailable = await labTestService.GetAllWithIncludeAsync(new List<string>() { });
            
            for(int i = 0; i < cvm.LabTestsIdChosen.Count; i++)
            {
                if (cvm.LabTestsIdChosen[i] == true)
                {
                    SaveLabResultViewModel LRVM = new();
                    LRVM.AppointmentId = Appointment.Id;
                    LRVM.Appointment = await appointmentService.SaveToVM(Appointment);
                    LRVM.LabTestId = LabTestAvailable[i].Id;
                    LRVM.Completed = false;
                    LRVM.Result = "";
                    await labResultService.Add(LRVM);
                }
            }
            Appointment.Status = AppointmentStatus.PendingResults;
            await appointmentService.Update(Appointment);
            return RedirectToRoute(new { Action = "Index", Controller = "Appointment" });
        }

        public async Task<IActionResult> ConsulteResults(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var list = await labResultService.GetAllWithIncludeAsync(new List<string>() { "Appointment", "LabTest" });
            list = list.Where(e=> e.Deleted != true).ToList();
            var labResult = list.FindAll(x => x.Appointment.Id == Id);
            return View(labResult);
        }
        
        public async Task<IActionResult> CompleteAppointment(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var Appointment = await appointmentService.GetById(Id);
            Appointment.Status = AppointmentStatus.Completed;
            await appointmentService.Update(Appointment);
            return RedirectToRoute(new { Action = "Index", Controller = "Appointment" });
        }
        public async Task<IActionResult> SeeResults(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var Appointment = await appointmentService.GetById(Id);
            var list = await labResultService.GetAllWithIncludeAsync(new List<string>() { "Appointment", "LabTest" });
            list = list.Where(e => e.Deleted != true).ToList();
            var labResult = list.FindAll(x => x.Appointment.Id == Id);
            return View(labResult);
        }
    }
}
