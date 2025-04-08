using PeopleManager.Application.Interfaces.Persistence;
using PeopleManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PeopleManager.Infrastructure.Persistence
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        // Static list for demo purpose In-Memory
        protected static readonly List<TEntity> _dataStore = new List<TEntity>();

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Simulating asynchronous operation
            await Task.Yield();

            // Logic processing ID if you are guid and have not been assigned (ID == guid.empty)
            if (typeof(TKey) == typeof(Guid) && EqualityComparer<TKey>.Default.Equals(entity.Id, default(TKey)!))
            {
                // because of  TEntity : IEntity<TKey>, we know entity has property Id
                // Safety way to assign new guid without needing hard TEntity
                dynamic dynamicEntity = entity;
                dynamicEntity.Id = Guid.NewGuid();
            }
            // add Logic for id other types if needed(int, long...) 

            _dataStore.Add(entity);
            return entity;
        }

        public async Task<bool> DeleteAsync(TKey id)
        {
            await Task.Yield();

            // find entity base on Id using Equals() of TKey (Guaranteed by IEquatable<TKey>)
            var entityToRemove = _dataStore.FirstOrDefault(e => e.Id.Equals(id));

            if (entityToRemove != null)
            {
                _dataStore.Remove(entityToRemove);
                return true; // delete success
            }

            return false; // not found 
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            await Task.Yield();

            // Compile expression to Func and use Where of LINQ to Objects
            var results = _dataStore.Where(predicate.Compile()).ToList();
            return results;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            await Task.Yield();
            return _dataStore.ToList();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
        {
            await Task.Yield();
            var entity = _dataStore.FirstOrDefault(e => e.Id.Equals(id));
            return entity;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await Task.Yield();

            var existingEntityIndex = _dataStore.FindIndex(e => e.Id.Equals(entity.Id));

            if (existingEntityIndex != -1) // Found entity
            {
                //replace old entity with new one
                _dataStore[existingEntityIndex] = entity;
                return true;
            }

            return false;
        }


        // SeedData giờ nên là một phần của Infrastructure, có thể tách ra class riêng
        public static async Task SeedDataAsync(IEnumerable<TEntity> data)
        {
            foreach (var item in data)
            {
                // Tạm gọi trực tiếp AddAsync của class này cho demo
                // Trong thực tế, seeding có thể phức tạp hơn
                await new Repository<TEntity, TKey>().AddAsync(item);
            }
        }
    }
}
