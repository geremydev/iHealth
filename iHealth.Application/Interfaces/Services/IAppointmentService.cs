using iHealth.Core.Application.Interfaces.Services;
using iHealth.Core.Application.ViewModels.AppointmentViewModel;
using iHealth.Core.Application.ViewModels.LabResultViewModel;
using iHealth.Core.Domain.Entities;

namespace iHealth.Core.Application.Interfaces.Repositories
{
    public interface IAppointmentService : IGenericService<Appointment, AppointmentViewModel,SaveAppointmentViewModel>
    {
        public Task PerformTests(LabResultViewModel vm);
        public Task CompleteAppointment(LabResultViewModel vm);
        public Task<List<LabResultViewModel>> GetLabResults(LabResultViewModel vm);
        public Task<List<AppointmentViewModel>> GetAllWithIncludeAsync(List<string> properties);

    }
}
