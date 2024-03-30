using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.ViewModels.LabResultViewModel;
using iHealth.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using iHealth.Core.Application.Helpers;
using iHealth.Core.Application.ViewModels.DoctorViewModels;
using iHealth.Core.Application.Interfaces.Repositories.Persons;
using iHealth.Core.Application.ViewModels.UserViewModels;
using System.Collections.Generic;
using iHealth.Core.Application.ViewModels.PatientViewModels;

namespace iHealth.Core.Application.Services
{
    public class LabResultService : ILabResultService
    {
        private readonly ILabResultRepository _labResultRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LabResultService(ILabResultRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            _labResultRepository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<SaveLabResultViewModel> Add(SaveLabResultViewModel vm)
        {
            UserViewModel User = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var model = await VMToModel(await SaveToVM(vm));
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = User.Name;
            model = await _labResultRepository.SaveAsync(model);
            return await VMToSave(await ModToViewModel(model));
        }

        public async Task<List<LabResultViewModel>> GetAll()
        {
            var list = await _labResultRepository.GetAllAsync();
            list = list.Where(e => e.Deleted != true).ToList();
            var newlist = new List<LabResultViewModel>();

            foreach (var element in list)
            {
                newlist.Add(await ModToViewModel(element));
            };
            newlist = newlist.FindAll(e => e.Deleted == false);
            return newlist;
        }

        public async Task<SaveLabResultViewModel> GetById(int Id)
        {
            return await VMToSave(await ModToViewModel(await _labResultRepository.GetEntityByIDAsync(Id)));
        }

        public async Task Update(SaveLabResultViewModel vm)
        {
            UserViewModel user = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var entity = await _labResultRepository.GetEntityByIDAsync(vm.Id);
            entity.Completed = vm.Completed;
            entity.Result = vm.Result;
            entity.LastModifiedDate = DateTime.Now;
            entity.LastModifiedBy = user.Name;
            await _labResultRepository.UpdateAsync(entity, entity.Id);
        }

        public async Task Delete(int Id)
        {
            UserViewModel user = _httpContextAccessor.HttpContext.Session.Get<UserViewModel>("user");
            var entity = await _labResultRepository.GetEntityByIDAsync(Id);
            entity.LastModifiedDate = DateTime.Now;
            entity.LastModifiedBy = user.Name;
            await _labResultRepository.DeleteAsync(entity);
        }

        

        



        public async Task<List<LabResultViewModel>> GetAllWithIncludeAsync(List<string> properties)
        {
            var list = await _labResultRepository.GetAllWithIncludeAsync(properties);
            list = list.Where(e => e.LabTest.Deleted != true).ToList();
            var newlist = new List<LabResultViewModel>();

            foreach (var element in list)
                newlist.Add(await ModToViewModel(element));
            return newlist;
        }

        public async Task<List<LabResultViewModel>> GetAllWithFilter(FilterViewModel filters, List<PatientViewModel> Patients = null)
        {
            //punto importante
            var labResults = await _labResultRepository.GetAllWithIncludeAsync(new List<string>() { "Appointment", "LabTest"});
            labResults = labResults.Where(e => e.Deleted != true).ToList();
            labResults = labResults.Where(e => e.LabTest.Deleted != true).ToList();
            if(Patients != null)
            {
                foreach (var patient in labResults)
                {
                    var entity = Patients.Find(e => e.Id == patient.Appointment.PatientId);
                    patient.Appointment.Patient = new Patient()
                    {
                        IdCard = entity.IdCard,
                        Name = entity.Name,
                        LastName = entity.LastName,
                        Email = entity.Email
                    };
                }
            }
            
            labResults = labResults.Where(s => s.Deleted == false).ToList();
            labResults = labResults.Where(s => s.Completed == false).ToList();
            var labResultsVM = new List<LabResultViewModel>();
            foreach (var item in labResults)
                labResultsVM.Add(await ModToViewModel(item));

            if (filters.PatientId != null)
            {
                labResultsVM = labResultsVM.Where(s => s.Appointment.PatientId == filters.PatientId).ToList();
            }
            if (filters.DoctorId != null)
            {
                labResultsVM = labResultsVM.Where(s =>
                    (s.Appointment.DoctorId == filters.DoctorId)).ToList();
            }
            if (filters.PatientIdCard != null)
            {
                labResultsVM = labResultsVM.Where(s => s.Appointment.Patient.IdCard == filters.PatientIdCard).ToList();
            }
            return labResultsVM;
        }




        public async Task<SaveLabResultViewModel> VMToSave(LabResultViewModel vm)
        {
            return new SaveLabResultViewModel()
            {
                AppointmentId = vm.AppointmentId,
                Completed = vm.Completed,
                Id = vm.Id,
                LabTestId = vm.LabTestId,
                Result = vm.Result
            };
        }

        public async Task<LabResultViewModel> SaveToVM(SaveLabResultViewModel sm)
        {
            return new LabResultViewModel()
            {
                AppointmentId = sm.AppointmentId,
                Completed = sm.Completed,
                Id = sm.Id,
                LabTestId = sm.LabTestId,
                Result = sm.Result
            };
        }
        
        public async Task<LabResult> VMToModel(LabResultViewModel vm)
        {
            return new LabResult()
            {
                Appointment = vm.Appointment,
                AppointmentId = vm.AppointmentId,
                Completed = vm.Completed,
                CreatedBy = vm.CreatedBy,
                CreatedDate = vm.CreatedDate,
                DeletedBy = vm.DeletedBy,
                DeletedDate = vm.DeletedDate,
                Deleted = vm.Deleted,
                Id = vm.Id,
                LabTestId = vm.LabTestId,
                LabTest = vm.LabTest,
                LastModifiedBy = vm.LastModifiedBy,
                LastModifiedDate = vm.LastModifiedDate,
                Result = vm.Result,
            };
        }

        public async Task<LabResultViewModel> ModToViewModel(LabResult m)
        {
            return new LabResultViewModel()
            {
                Appointment = m.Appointment,
                AppointmentId = (int)m.AppointmentId,
                Completed = m.Completed,
                CreatedBy = m.CreatedBy,
                CreatedDate = m.CreatedDate,
                DeletedBy = m.DeletedBy,
                DeletedDate = m.DeletedDate,
                Deleted = m.Deleted,
                Id = m.Id,
                LabTestId = (int)m.LabTestId,
                LabTest = m.LabTest,
                LastModifiedBy = m.LastModifiedBy,
                LastModifiedDate = m.LastModifiedDate,
                Result = m.Result,
            };
        }

    }
}
