using iHealth.Core.Application.ViewModels.PatientViewModels;
using iHealth.Core.Domain.Entities;

namespace iHealth.Core.Application.Interfaces.Services
{
    public interface IPatientService : IGenericService<Patient, PatientViewModel, SavePatientViewModel>
    {
        public Task<List<PatientViewModel>> GetAllWithIncludeAsync(List<string> properties);
    }
}
