using iHealth.Core.Application.ViewModels.LabResultViewModel;
using iHealth.Core.Application.ViewModels.PatientViewModels;
using iHealth.Core.Domain.Entities;

namespace iHealth.Core.Application.Interfaces.Services
{
    public interface ILabResultService : IGenericService<LabResult, LabResultViewModel, SaveLabResultViewModel>
    {
        public Task<List<LabResultViewModel>> GetAllWithIncludeAsync(List<string> properties);
        public Task<List<LabResultViewModel>> GetAllWithFilter(FilterViewModel filters, List<PatientViewModel> Patients = null);
    }
}
