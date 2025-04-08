using PeopleManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleManager.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync<TEntity, TKey>(
            IEnumerable<TEntity> data,
            Func<TEntity, Task<TEntity>> addAction)
            where TEntity : class, IEntity<TKey>
            where TKey : IEquatable<TKey>
        {
            if (addAction == null) throw new ArgumentNullException(nameof(addAction));

            Console.WriteLine($"Attempting to seed {data.Count()} items of type {typeof(TEntity).Name}...");
            foreach (var item in data)
            {
                await addAction(item);
            }
            Console.WriteLine($"Seeding for {typeof(TEntity).Name} completed.");
        }
    }
}
