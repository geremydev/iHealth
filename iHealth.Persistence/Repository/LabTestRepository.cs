using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Domain.Entities;
using iHealth.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace iHealth.Infrastructure.Persistence.Repository
{
    public class LabTestRepository : GenericRepository<LabTest>, ILabTestRepository
    {
        private readonly iHealthContext _context;
        private readonly DbSet<LabTest> _entities;
        public LabTestRepository(iHealthContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<LabTest>();
        }

    }
}
