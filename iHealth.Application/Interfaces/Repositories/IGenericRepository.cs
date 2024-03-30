using System.Linq.Expressions;

namespace iHealth.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> SaveAsync(TEntity entity);
        Task UpdateAsync(TEntity entity, int Id);
        Task DeleteAsync(TEntity entity);
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetEntityByIDAsync(int ID);
        Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> filter);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
        Task<List<TEntity>> GetAllWithIncludeAsync(List<string> properties);

    }
}
