using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;

namespace NewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, IBaseEntity
    {
        protected readonly NewsAggregatorContext Db;

        protected readonly DbSet<T> Table;

        protected BaseRepository(NewsAggregatorContext context)
        {
            Db = context;
            Table = Db.Set<T>(); // return table with type T

        }
        public async Task<T> GetById(Guid id)
        {
            return await Table.FirstOrDefaultAsync(obj => obj.Id.Equals(id));
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate,
            params Expression<Func<T, object>>[] includes)
        {
            var result = Table.Where(predicate);
            if (includes.Any())
            {
                result = includes
                    .Aggregate(result,
                        (current, include)
                            => current.Include(include));
            }

            return result;
        }

        public IQueryable<T> GetAll()
        {
            return Table;
        }


        public async Task Add(T add)
        {
            await Table.AddAsync(add);
        }

        public async Task AddRange(IEnumerable<T> addRange)
        {
            await Table.AddRangeAsync(addRange);

        }

        public async Task Update(T update)
        {
            Table.Update(update);
        }

        public async Task Remove(T remove)
        {
            Table.Remove(remove);

        }

        public void RemoveRange(IEnumerable<T> removeRange)
        {
            Table.RemoveRange(removeRange);
        }

        public void Dispose()
        {
            Db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}