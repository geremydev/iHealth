using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.ViewModels.AppointmentViewModel;
using iHealth.Core.Application.ViewModels.LabResultViewModel;
using iHealth.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using iHealth.Core.Application.Helpers;
using iHealth.Core.Application.Interfaces.Repositories.Persons;
using iHealth.Core.Application.ViewModels.DoctorViewModels;
using iHealth.Core.Application.ViewModels.UserViewModels;

namespace iHealth.Core.Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppointmentService(IAppointmentRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentRepository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SaveAppointmentViewModel> Add(SaveAppointmentViewModel vm)
        {
            UserViewModel User = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var model = await VMToModel(await SaveToVM(vm));
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = User.Name;
            model = await _appointmentRepository.SaveAsync(model);

            return await VMToSave(await ModToViewModel(model));
        }

        public async Task<List<AppointmentViewModel>> GetAll()
        {
            var list = await _appointmentRepository.GetAllAsync();
            var newlist = new List<AppointmentViewModel>();
            foreach (var element in list)
            {
                newlist.Add(await ModToViewModel(element));
            };
            newlist = newlist.FindAll(e => e.Deleted == false);
            return newlist;
        }

        public async Task<SaveAppointmentViewModel> GetById(int Id)
        {
            return await VMToSave(await ModToViewModel(await _appointmentRepository.GetEntityByIDAsync(Id)));
        }
        public async Task Update(SaveAppointmentViewModel vm)
        {
            UserViewModel user = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var entity = await _appointmentRepository.GetEntityByIDAsync(vm.Id);
            entity.LastModifiedDate = DateTime.Now;
            entity.LastModifiedBy = user.Name;
            entity.Status = (Domain.Entities.AppointmentStatus)vm.Status;
            await _appointmentRepository.UpdateAsync(entity, entity.Id);
        }
        public async Task Delete(int Id)
        {
            UserViewModel user = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var entity = await _appointmentRepository.GetEntityByIDAsync(Id);
            entity.DeletedDate = DateTime.Now;
            entity.DeletedBy = user.Name;
            entity.Deleted = true;
            await _appointmentRepository.DeleteAsync(entity);
        }

        public async Task<List<AppointmentViewModel>> GetAllWithIncludeAsync(List<string> properties)
        {
            var list = await _appointmentRepository.GetAllWithIncludeAsync(properties);
            list = list.Where(e => e.Deleted != true).ToList();
            var newlist = new List<AppointmentViewModel>();

            foreach (var element in list)
                newlist.Add(await ModToViewModel(element));
            return newlist;
        }






        public Task<List<LabResultViewModel>> GetLabResults(LabResultViewModel vm)
        {
            throw new NotImplementedException();
        }
        public async Task PerformTests(LabResultViewModel vm)
        {
            throw new NotImplementedException();
        }
        public Task CompleteAppointment(LabResultViewModel vm)
        {
            throw new NotImplementedException();
        }





        public async Task<Appointment> VMToModel(AppointmentViewModel vm)
        {
            return new Appointment()
            {
                AppointmentDate = vm.AppointmentDate,
                CreatedBy = vm.CreatedBy,
                CreatedDate = vm.CreatedDate,
                DeletedBy = vm.DeletedBy,
                DeletedDate = vm.DeletedDate,
                Deleted = vm.Deleted,
                Doctor = vm.Doctor,
                DoctorId = vm.DoctorId,
                Id = vm.Id,
                LabResult = vm.LabResult,
                LastModifiedBy = vm.LastModifiedBy,
                LastModifiedDate = vm.LastModifiedDate,
                MedicalConcern = vm.MedicalConcern,
                Patient = vm.Patient,
                PatientId = vm.PatientId,
                Status = (Domain.Entities.AppointmentStatus)vm.Status
            };
        }

        public async Task<AppointmentViewModel> ModToViewModel(Appointment m)
        {
            return new AppointmentViewModel()
            {
                AppointmentDate = m.AppointmentDate,
                CreatedBy = m.CreatedBy,
                CreatedDate = m.CreatedDate,
                DeletedBy = m.DeletedBy,
                DeletedDate = m.DeletedDate,
                Deleted = m.Deleted,
                Doctor = m.Doctor,
                DoctorId = m.DoctorId,
                Id = m.Id,
                LabResult = m.LabResult,
                LastModifiedBy = m.LastModifiedBy,
                LastModifiedDate = m.LastModifiedDate,
                MedicalConcern = m.MedicalConcern,
                Patient = m.Patient,
                PatientId = m.PatientId,
                Status = (ViewModels.AppointmentViewModel.AppointmentStatus)m.Status
            };
        }

        public async Task<SaveAppointmentViewModel> VMToSave(AppointmentViewModel vm)
        {
            return new SaveAppointmentViewModel()
            {
                AppointmentDate = vm.AppointmentDate,
                DoctorId = vm.DoctorId,
                Id = vm.Id,
                PatientId = vm.PatientId,
                Status = (ViewModels.AppointmentViewModel.AppointmentStatus)vm.Status,
                MedicalConcern = vm.MedicalConcern
            };
        }

        public async Task<AppointmentViewModel> SaveToVM(SaveAppointmentViewModel vm)
        {
            return new AppointmentViewModel()
            {
                AppointmentDate = vm.AppointmentDate,
                DoctorId = vm.DoctorId,
                Id = vm.Id,
                PatientId = vm.PatientId,
                Status = vm.Status,
                MedicalConcern = vm.MedicalConcern
            };
        }
    }
}
