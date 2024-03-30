using iHealth.Core.Application.ViewModels.LabTestViewModel;
using iHealth.Core.Domain.Entities;

namespace iHealth.Core.Application.Interfaces.Services
{
    public interface ILabTestService : IGenericService<LabTest, LabTestViewModel, SaveLabTestViewModel>
    {
        public Task<List<LabTestViewModel>> GetAllWithIncludeAsync(List<string> properties);
    }
}
