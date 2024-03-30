
using iHealth.Core.Application.Interfaces.Repositories.Persons;
using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.ViewModels.DoctorViewModels;
using iHealth.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using iHealth.Core.Application.Helpers;
using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Application.ViewModels.UserViewModels;

namespace iHealth.Core.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DoctorService(IDoctorRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _doctorRepository = repository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<SaveDoctorViewModel> Add(SaveDoctorViewModel vm)
        {
            UserViewModel User = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var model = await VMToModel(await SaveToVM(vm));
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = User.Name;
            model = await _doctorRepository.SaveAsync(model);
            return await VMToSave(await ModToViewModel(model));
        }

        public async Task<List<DoctorViewModel>> GetAll()
        {
            var list = await _doctorRepository.GetAllAsync();
            var newlist = new List<DoctorViewModel>();

            foreach (var element in list)
            {
                newlist.Add(await ModToViewModel(element));
            };
            newlist = newlist.FindAll(e => e.Deleted == false);
            return newlist;
        }
        
        public async Task<SaveDoctorViewModel> GetById(int Id)
        {
            return await VMToSave(await ModToViewModel(await _doctorRepository.GetEntityByIDAsync(Id)));
        }
        
        public async Task Update(SaveDoctorViewModel vm)
        {
            UserViewModel user = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var entity = await _doctorRepository.GetEntityByIDAsync(vm.Id);
            entity.LastModifiedDate = DateTime.Now;
            entity.LastModifiedBy = user.Name;
            entity.ImageURL = vm.ImageURL;
            entity.Email = vm.Email;
            entity.IdCard = vm.IdCard;
            entity.Phone = vm.Phone;
            entity.Name = vm.Name;
            entity.LastName = vm.LastName;
            await _doctorRepository.UpdateAsync(entity, entity.Id);
        }

        public async Task Delete(int Id)
        {
            UserViewModel user = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var entity = await _doctorRepository.GetEntityByIDAsync(Id);
            entity.Deleted = true;
            entity.DeletedBy = user.Name;
            entity.DeletedDate = DateTime.Now;
            await _doctorRepository.DeleteAsync(entity);
        }

        



        public async Task<List<DoctorViewModel>> GetAllWithIncludeAsync(List<string> properties)
        {
            var list = await _doctorRepository.GetAllWithIncludeAsync(properties);
            list = list.Where(e => e.Deleted != true).ToList();
            var newlist = new List<DoctorViewModel>();

            foreach (var element in list)
                newlist.Add(await ModToViewModel(element));
            return newlist;
        }
        





        public async Task<Doctor> VMToModel(DoctorViewModel vm)
        {
            return new Doctor()
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
                ImageURL = vm.ImageURL
            };
        }

        public async Task<DoctorViewModel> ModToViewModel(Doctor m)
        {
            return new DoctorViewModel()
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
                ImageURL = m.ImageURL
            };
        }
        public async Task<SaveDoctorViewModel> VMToSave(DoctorViewModel vm)
        {
            return new SaveDoctorViewModel()
            {
                Email = vm.Email,
                IdCard = vm.IdCard,
                ImageURL = vm.ImageURL,
                LastName = vm.LastName,
                Name = vm.Name,
                Phone = vm.Phone,
                Id = vm.Id
            };
        }

        public async Task<DoctorViewModel> SaveToVM(SaveDoctorViewModel sm)
        {
            return new DoctorViewModel()
            {
                Email = sm.Email,
                IdCard = sm.IdCard,
                ImageURL = sm.ImageURL,
                LastName = sm.LastName,
                Name = sm.Name,
                Phone = sm.Phone,
                Id = sm.Id
            };
        }
    }
}
