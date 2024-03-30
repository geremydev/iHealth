using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Application.ViewModels.AppointmentViewModel;
using iHealth.Core.Domain.Entities;
using iHealth.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace iHealth.Infrastructure.Persistence.Repository
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly iHealthContext _context;
        private readonly DbSet<Appointment> _entities;
        public AppointmentRepository(iHealthContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<Appointment>();
        }
        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<Appointment> GetEntityByIDAsync(int Id)
        {
            return await _entities.FindAsync(Id);
        }

        public async Task<Appointment> ConvertToModel(AppointmentViewModel vm)
        {
            return new Appointment()
            {
                Id = vm.Id,
                PatientId = vm.PatientId,
                DoctorId = vm.DoctorId,
                AppointmentDate = vm.AppointmentDate,
                MedicalConcern = vm.MedicalConcern,
                Patient = vm.Patient,
                Doctor = vm.Doctor,
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

        public async Task<AppointmentViewModel> ConvertToViewModel(Appointment m)
        {
            return new AppointmentViewModel()
            {
                Id = m.Id,
                PatientId = m.PatientId,
                DoctorId = m.DoctorId,
                AppointmentDate = m.AppointmentDate,
                MedicalConcern = m.MedicalConcern,
                Patient = m.Patient,
                Doctor = m.Doctor,
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
