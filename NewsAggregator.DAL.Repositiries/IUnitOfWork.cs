using System;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;

namespace NewsAggregator.DAL.Repositories.Implementation
{
    public interface IUnitOfWork: IDisposable
    {
        IBaseRepository<News> News { get; }
        IBaseRepository<RssSource> Rss { get; }
        IBaseRepository<Role> Roles { get; }
        IBaseRepository<User> Users { get; }
        IBaseRepository<Comment> Comments { get; }
         
        Task<int> SaveChangesAsync();
    }
}