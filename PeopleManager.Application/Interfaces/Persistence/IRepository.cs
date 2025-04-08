using PeopleManager.Domain.Interfaces;
using System.Linq.Expressions;

namespace PeopleManager.Application.Interfaces.Persistence
{
    public interface IRepository<TEntity, TKey>
         where TEntity : class, IEntity<TKey>
         where TKey : IEquatable<TKey>
    {
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> AddAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TKey id);
    }
}