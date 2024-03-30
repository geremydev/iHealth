using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Application.ViewModels.PatientViewModels;
using iHealth.Core.Domain.Entities;
using iHealth.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace iHealth.Infrastructure.Persistence.Repository
{
    public class PatientRepository : GenericRepository<Patient>, IPatientRepository
    {
        private readonly iHealthContext _context;
        private readonly DbSet<Patient> _entities;
        public PatientRepository(iHealthContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<Patient>();
        }

        public async override Task<List<Patient>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public override async Task<Patient> GetEntityByIDAsync(int Id)
        {
            return await _entities.FindAsync(Id);
        }

        public async Task<Patient> ConvertToModel(PatientViewModel vm)
        {
            return new Patient()
            {
                Id = vm.Id,
                Name = vm.Name,
                Email = vm.Email,
                LastName = vm.LastName,
                ImageURL = vm.ImageURL,
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

        public async Task<PatientViewModel> ConvertToViewModel(Patient m)
        {
            return new PatientViewModel()
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
