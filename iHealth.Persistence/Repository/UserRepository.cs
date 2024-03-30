using iHealth.Core.Application.Helpers;
using iHealth.Core.Application.Interfaces.Repositories;
using iHealth.Core.Domain.Entities;
using iHealth.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace iHealth.Infrastructure.Persistence.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly iHealthContext _context;
        private readonly DbSet<User> _entities;
        public UserRepository(iHealthContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<User>();
        }

        public async override Task<List<User>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async override Task DeleteAsync(User entity)
        {
            entity.Deleted = true;
            entity.DeletedDate = DateTime.Now;
            //El usuario que borró se pasará desde arriba, se pondrá en su atributo desde el servicio o desde el mismo 
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async override Task<User> SaveAsync(User entity)
        {
            entity.Password = PasswordEncryption.ComputeSha256Hash(entity.Password);
            await _entities.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
