using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Interfaces
{
    public interface IBaseRepository<T> : IDisposable where T : class, IBaseEntity
    {
        Task<T> GetById(Guid id);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, 
            params Expression<Func<T, object>>[] includes);

        IQueryable<T> GetAllNews();
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        
        Task Update(T entity);

        Task Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}