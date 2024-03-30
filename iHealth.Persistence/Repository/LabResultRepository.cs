using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Application.ViewModels.LabResultViewModel;
using iHealth.Core.Domain.Entities;
using iHealth.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace iHealth.Infrastructure.Persistence.Repository
{
    public class LabResultRepository : GenericRepository<LabResult>, ILabResultRepository
    {
        private readonly iHealthContext _context;
        private readonly DbSet<LabResult> _entities;
        public LabResultRepository(iHealthContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<LabResult>();
        }

        public async Task<List<LabResult>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<LabResult> GetEntityByIDAsync(int Id)
        {
            return await _entities.FindAsync(Id);
        }

        public async Task<LabResult> ConvertToModel(LabResultViewModel vm)
        {
            return new LabResult()
            {
                Id = vm.Id,
                AppointmentId = vm.AppointmentId,
                LabTestId = vm.LabTestId,
                Result = vm.Result,
                Completed = vm.Completed,
                Appointment = vm.Appointment,
                LabTest = vm.LabTest,
                //Auditory
                CreatedBy = vm.CreatedBy,
                CreatedDate = vm.CreatedDate,
                DeletedBy = vm.DeletedBy,
                DeletedDate = vm.DeletedDate,
                Deleted = vm.Deleted,
                LastModifiedBy = vm.LastModifiedBy,
                LastModifiedDate = vm.LastModifiedDate
            };
        }

        public async Task<LabResultViewModel> ConvertToViewModel(LabResult m)
        {
            return new LabResultViewModel()
            {
                Id = m.Id,
                AppointmentId = (int)m.AppointmentId,
                LabTestId = (int)m.LabTestId,
                Result = m.Result,
                Completed = m.Completed,
                Appointment = m.Appointment,
                LabTest = m.LabTest,
                //Auditory
                CreatedBy = m.CreatedBy,
                CreatedDate = m.CreatedDate,
                DeletedBy = m.DeletedBy,
                DeletedDate = m.DeletedDate,
                Deleted = m.Deleted,
                LastModifiedBy = m.LastModifiedBy,
                LastModifiedDate = m.LastModifiedDate
            };
        }

    }
}
