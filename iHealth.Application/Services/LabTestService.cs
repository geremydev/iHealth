using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.ViewModels.LabTestViewModel;
using iHealth.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using iHealth.Core.Application.Helpers;
using iHealth.Core.Application.ViewModels.UserViewModels;

namespace iHealth.Core.Application.Services
{
    public class LabTestService : ILabTestService
    {
        private readonly ILabTestRepository _labTestRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LabTestService(ILabTestRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _labTestRepository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SaveLabTestViewModel> Add(SaveLabTestViewModel vm)
        {
            UserViewModel User = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var model = await VMToModel(await SaveToVM(vm));
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = User.Name;
            model = await _labTestRepository.SaveAsync(model);
            return await VMToSave(await ModToViewModel(model));
        }

        public async Task<List<LabTestViewModel>> GetAll()
        {
            var list = await _labTestRepository.GetAllAsync();
            list = list.Where(e=>e.Deleted != true).ToList();
            var newlist = new List<LabTestViewModel>();

            foreach (var element in list)
            {
                newlist.Add(await ModToViewModel(element));
            };

            newlist = newlist.FindAll(e => e.Deleted == false);
            return newlist;
        }

        public async Task<SaveLabTestViewModel> GetById(int Id)
        {
            return await VMToSave(await ModToViewModel(await _labTestRepository.GetEntityByIDAsync(Id)));
        }

        public async Task Update(SaveLabTestViewModel vm)
        {
            UserViewModel user = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var entity = await _labTestRepository.GetEntityByIDAsync(vm.Id);
            entity.LastModifiedDate = DateTime.Now;
            entity.LastModifiedBy = user.Name;
            entity.Name = vm.Name;
            await _labTestRepository.UpdateAsync(entity, entity.Id);
        }

        public async Task Delete(int Id)
        {
            UserViewModel user = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var entity = await _labTestRepository.GetEntityByIDAsync(Id);
            entity.DeletedDate = DateTime.Now;
            entity.DeletedBy = user.Name;
            entity.Deleted = true;
            await _labTestRepository.DeleteAsync(entity);
        }

        

        public async Task<List<LabTestViewModel>> GetAllWithIncludeAsync(List<string> properties)
        {
            var list = await _labTestRepository.GetAllWithIncludeAsync(properties);
            list = list.Where(e => e.Deleted != true).ToList();
            var newlist = new List<LabTestViewModel>();

            foreach (var element in list)
                newlist.Add(await ModToViewModel(element));
            return newlist;
        }

        public async Task<SaveLabTestViewModel> VMToSave(LabTestViewModel vm)
        {
            return new SaveLabTestViewModel()
            {
                Id = vm.Id,
                LabResults = vm.LabResults,
                Name = vm.Name,
            };
        }

        public async Task<LabTestViewModel> SaveToVM(SaveLabTestViewModel sm)
        {
            return new LabTestViewModel()
            {
                Id = sm.Id,
                LabResults = sm.LabResults,
                Name = sm.Name,
            };
        }
        
        public async Task<LabTest> VMToModel(LabTestViewModel vm)
        {
            return new LabTest()
            {
                CreatedBy = vm.CreatedBy,
                CreatedDate = vm.CreatedDate,
                DeletedBy = vm.DeletedBy,
                DeletedDate = vm.DeletedDate,
                Deleted = vm.Deleted,
                Id = vm.Id,
                LabResults = vm.LabResults,
                LastModifiedBy = vm.LastModifiedBy,
                LastModifiedDate = vm.LastModifiedDate,
                Name = vm.Name
            };
        }

        public async Task<LabTestViewModel> ModToViewModel(LabTest m)
        {
            return new LabTestViewModel()
            {
                CreatedBy = m.CreatedBy,
                CreatedDate = m.CreatedDate,
                DeletedBy = m.DeletedBy,
                DeletedDate = m.DeletedDate,
                Deleted = m.Deleted,
                Id = m.Id,
                LabResults = m.LabResults,
                LastModifiedBy = m.LastModifiedBy,
                LastModifiedDate = m.LastModifiedDate,
                Name = m.Name
            };
        }
    }
}
