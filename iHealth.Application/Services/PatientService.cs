using iHealth.Core.Application.ViewModels.PatientViewModels;
using Microsoft.AspNetCore.Http;
using iHealth.Core.Application.Helpers;
using iHealth.Core.Application.ViewModels.UserViewModels;
using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Domain.Entities;


namespace iHealth.Core.Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientService(IPatientRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _patientRepository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SavePatientViewModel> Add(SavePatientViewModel vm)
        {
            UserViewModel user = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var model = await VMToModel(await SaveToVM(vm));
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = user.Name;
            model = await _patientRepository.SaveAsync(model);
            return await VMToSave(await ModToViewModel(model));
        }

        public async Task<List<PatientViewModel>> GetAll()
        {
            var list = await _patientRepository.GetAllAsync();
            var newlist = new List<PatientViewModel>();

            foreach (var element in list)
            {
                newlist.Add(await ModToViewModel(element));
            };

            newlist = newlist.FindAll(e => e.Deleted == false);
            return newlist;
        }

        public async Task<SavePatientViewModel> GetById(int Id)
        {
            return await VMToSave(await ModToViewModel(await _patientRepository.GetEntityByIDAsync(Id)));

        }

        public async Task Update(SavePatientViewModel sm)
        {
            UserViewModel user = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var entity = await _patientRepository.GetEntityByIDAsync(sm.Id);
            entity.LastModifiedDate = DateTime.Now;
            entity.LastModifiedBy = user.Name;
            entity.Name = sm.Name;
            entity.LastName = sm.LastName;
            entity.Address = sm.Address;
            entity.Smoker = sm.Smoker;
            entity.Email = sm.Email;
            entity.Phone = sm.Phone;
            entity.Allergies = sm.Allergies;
            entity.IdCard = sm.IdCard;
            entity.BirthDate = sm.BirthDate;
            entity.ImageURL = sm.ImageURL;

            await _patientRepository.UpdateAsync(entity, entity.Id);
        }

        public async Task Delete(int Id)
        {
            UserViewModel user = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var entity = await _patientRepository.GetEntityByIDAsync(Id);
            entity.Deleted = true;
            entity.DeletedDate = DateTime.Now;
            entity.DeletedBy = user.Name;
            await _patientRepository.DeleteAsync(entity);
        }

        

        

        

        
        public async Task<List<PatientViewModel>> GetAllWithIncludeAsync(List<string> properties)
        {
            var list = await _patientRepository.GetAllWithIncludeAsync(properties);
            list = list.Where(e => e.Deleted != true).ToList();
            var newlist = new List<PatientViewModel>();

            foreach (var element in list)
                newlist.Add(await ModToViewModel(element));
            return newlist;
        }

        







        public async Task<SavePatientViewModel> VMToSave(PatientViewModel vm)
        {
            return new SavePatientViewModel()
            {
                Address = vm.Address,
                Allergies = vm.Allergies,
                BirthDate = vm.BirthDate,
                Email = vm.Email,
                Id = vm.Id,
                IdCard = vm.IdCard,
                ImageURL = vm.ImageURL,
                LastName = vm.LastName,
                Name = vm.Name,
                Phone = vm.Phone,
                Smoker = vm.Smoker
            };
        }

        public async Task<PatientViewModel> SaveToVM(SavePatientViewModel sm)
        {
            return new PatientViewModel()
            {
                Address = sm.Address,
                Allergies = sm.Allergies,
                BirthDate = sm.BirthDate,
                Email = sm.Email,
                Id = sm.Id,
                IdCard = sm.IdCard,
                ImageURL = sm.ImageURL,
                LastName = sm.LastName,
                Name = sm.Name,
                Phone = sm.Phone,
                Smoker = sm.Smoker
            };
        }

        public async Task<Patient> VMToModel(PatientViewModel vm)
        {
            return new Patient()
            {
                Name = vm.Name,
                Appointments = vm.Appointments,
                CreatedBy = vm.CreatedBy,
                CreatedDate = vm.CreatedDate,
                DeletedBy = vm.DeletedBy,
                DeletedDate = vm.DeletedDate,
                Deleted = vm.Deleted,
                Email = vm.Email,
                IdCard = vm.IdCard,
                Id = vm.Id,
                LastModifiedBy = vm.LastModifiedBy,
                LastModifiedDate = vm.LastModifiedDate,
                LastName = vm.LastName,
                Phone = vm.Phone,
                ImageURL = vm.ImageURL,
                Address = vm.Address,
                Allergies = vm.Allergies,
                Smoker = vm.Smoker,
                BirthDate = vm.BirthDate
            };
        }

        public async Task<PatientViewModel> ModToViewModel(Patient m)
        {
            return new PatientViewModel()
            {
                Name = m.Name,
                Appointments = m.Appointments,
                CreatedBy = m.CreatedBy,
                CreatedDate = m.CreatedDate,
                DeletedBy = m.DeletedBy,
                DeletedDate = m.DeletedDate,
                Deleted = m.Deleted,
                Email = m.Email,
                IdCard = m.IdCard,
                Id = m.Id,
                LastModifiedBy = m.LastModifiedBy,
                LastModifiedDate = m.LastModifiedDate,
                LastName = m.LastName,
                Phone = m.Phone,
                ImageURL = m.ImageURL,
                Address = m.Address,
                Allergies = m.Allergies,
                Smoker = m.Smoker,
                BirthDate = m.BirthDate

            };
        }
    }
}
