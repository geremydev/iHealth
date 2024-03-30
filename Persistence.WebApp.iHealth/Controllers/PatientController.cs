using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.ViewModels.PatientViewModels;
using iHealth.Middleware;
using Microsoft.AspNetCore.Mvc;
using Persistence.WebApp.iHealth.Controllers;

namespace iHealth.Controllers
{
    public class PatientController : Controller
    {

        private readonly ILogger<UserController> _logger;
        private readonly IPatientService patientService;
        private readonly ValidateUserSession validateUserSession;


        public PatientController(ILogger<UserController> logger, IPatientService service, ValidateUserSession validateUserSession)
        {
            _logger = logger;
            patientService = service;
            this.validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var list = await patientService.GetAll();
            list = list.FindAll(e => e.Deleted == false);
            return View(list);
        }

        public async Task<IActionResult> AddPatient()
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient(SavePatientViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var patient = await patientService.Add(vm);
            if(patient != null && patient.Id != 0)
            {
                patient.ImageURL = UploadFile(vm.Image, patient.Id);
                await patientService.Update(patient);
            }
            return RedirectToRoute(new { Action = "Index", Controller = "Patient" });
        }

        public async Task<IActionResult> DeletePatient(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var vm = await patientService.GetById(Id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePatient(SavePatientViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await patientService.Delete(vm.Id);
            return RedirectToRoute(new {Action="Index", Controller="Patient"});
        }

        public async Task<IActionResult> UpdatePatient(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var vm = await patientService.GetById(Id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePatient(SavePatientViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            SavePatientViewModel VM = await patientService.GetById(vm.Id);
            vm.ImageURL = UploadFile(vm.Image, vm.Id, true, VM.ImageURL);
            await patientService.Update(vm);
            return RedirectToRoute(new { Action = "Index", Controller = "Patient" });
        }

        public async Task<IActionResult> Details(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var list = await patientService.GetAllWithIncludeAsync(new List<string>() { "Appointments" });
            return View(list.Find(e => e.Id == Id));
        }

        private string UploadFile(IFormFile file, int Id, bool isEditMode = false, string imageUrl = "")
        {
            if (isEditMode && file == null)
            {
                return imageUrl;
            }
            //Get directory path
            string basePath = $"/Images/Patients/{Id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");

            //create folder if not exist
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            //Get file path
            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;

            string fileNameWithPath = Path.Combine(path, fileName);

            using(var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if(isEditMode)
            {
                string[] oldImagePart = imageUrl.Split("/");
                string oldImageName = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImageName);

                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }
            return $"{basePath}/{fileName}";
        }
    }
}
