using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.Services;
using iHealth.Core.Application.ViewModels.DoctorViewModels;
using iHealth.Core.Application.ViewModels.PatientViewModels;
using iHealth.Middleware;
using Microsoft.AspNetCore.Mvc;

namespace iHealth.Controllers
{
    public class DoctorController : Controller
    {
        private readonly ILogger<DoctorController> _logger;
        private readonly IDoctorService doctorService;
        private readonly ValidateUserSession validateUserSession;

        public DoctorController(ILogger<DoctorController> logger, IDoctorService service, ValidateUserSession validateUserSession)
        {
            _logger = logger;
            doctorService = service;
            this.validateUserSession = validateUserSession;
        }

        public async Task<IActionResult> Index()
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var list = await doctorService.GetAll();
            list = list.FindAll(e => e.Deleted == false);
            return View(list);
        }

        public async Task<IActionResult> AddDoctor()
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor(SaveDoctorViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var doctor = await doctorService.Add(vm);
            if (doctor != null && doctor.Id != 0)
            {
                doctor.ImageURL = UploadFile(vm.Image, doctor.Id);
                await doctorService.Update(doctor);
            }
            return RedirectToRoute(new { Action = "Index", Controller = "Doctor" });
        }

        public async Task<IActionResult> DeleteDoctor(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var vm = await doctorService.GetById(Id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDoctor(SaveDoctorViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await doctorService.Delete(vm.Id);
            return RedirectToRoute(new { Action = "Index", Controller = "Doctor" });
        }

        public async Task<IActionResult> UpdateDoctor(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var vm = await doctorService.GetById(Id);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDoctor(SaveDoctorViewModel vm)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            SaveDoctorViewModel VM = await doctorService.GetById(vm.Id);
            vm.ImageURL = UploadFile(vm.Image, vm.Id, true, VM.ImageURL);
            await doctorService.Update(vm);
            return RedirectToRoute(new { Action = "Index", Controller = "Doctor" });
        }

        public async Task<IActionResult> Details(int Id)
        {
            if (!validateUserSession.HasUser())
            {
                return RedirectToRoute(new { Controller = "Menu", Action = "Login" });
            }
            var list = await doctorService.GetAllWithIncludeAsync(new List<string>() { "Appointments" });
            return View(list.Find(e => e.Id == Id));
        }

        private string UploadFile(IFormFile file, int Id, bool isEditMode = false, string imageUrl = "")
        {
            if (isEditMode && file == null)
            {
                return imageUrl;
            }
            //Get directory path
            string basePath = $"/Images/Doctors/{Id}";
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

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode)
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
