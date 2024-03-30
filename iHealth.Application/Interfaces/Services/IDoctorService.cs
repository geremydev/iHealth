using iHealth.Core.Application.ViewModels.DoctorViewModels;
using iHealth.Core.Domain.Entities;

namespace iHealth.Core.Application.Interfaces.Services
{
    public interface IDoctorService : IGenericService<Doctor, DoctorViewModel, SaveDoctorViewModel>
    {
        public Task<List<DoctorViewModel>> GetAllWithIncludeAsync(List<string> properties);
    }
}
