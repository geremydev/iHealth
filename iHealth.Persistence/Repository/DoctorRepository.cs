using iHealth.Core.Application.Interfaces.Repositories.Persons;
using iHealth.Core.Application.ViewModels.DoctorViewModels;
using iHealth.Core.Domain.Entities;
using iHealth.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace iHealth.Infrastructure.Persistence.Repository
{
    public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
    {
        private readonly iHealthContext _context;
        private readonly DbSet<Doctor> _entities;
        public DoctorRepository(iHealthContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<Doctor>();
        }

        public async Task<List<Doctor>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<Doctor> GetEntityByIDAsync(int Id)
        {
            return await _entities.FindAsync(Id);
        }

        public async Task<Doctor> ConvertToModel(DoctorViewModel vm)
        {
            return new Doctor()
            {
                Id = vm.Id,
                Name = vm.Name,
                Email = vm.Email,
                LastName = vm.LastName,
                ImageURL = vm.ImageURL,
                IdCard = vm.IdCard,
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

        public async Task<DoctorViewModel> ConvertToViewModel(Doctor m)
        {
            return new DoctorViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                LastName = m.LastName,
                ImageURL = m.ImageURL,
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
