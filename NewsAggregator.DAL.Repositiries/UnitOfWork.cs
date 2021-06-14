using System;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;

namespace NewsAggregator.DAL.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly NewsAggregatorContext _db;
        private readonly IBaseRepository<News> _newsRepository;
        private readonly IBaseRepository<RssSource> _rssRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Comment> _commentRepository;

        public UnitOfWork(NewsAggregatorContext db,
            IBaseRepository<News> newsRepository,
            IBaseRepository<RssSource> rssRepository, 
            IBaseRepository<Role> roleRepository, 
            IBaseRepository<User> userRepository, 
            IBaseRepository<Comment> commentRepository)
        {
            _db = db;
            _newsRepository = newsRepository;
            _rssRepository = rssRepository;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
        }

        public IBaseRepository<News> News => _newsRepository;
        public IBaseRepository<RssSource> Rss => _rssRepository;
        public IBaseRepository<Role> Role => _roleRepository;
        public IBaseRepository<User> Users => _userRepository;
        public IBaseRepository<Comment> Comments => _commentRepository;

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }
        public void Dispose()
        {
            _db?.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public interface IUnitOfWork: IDisposable
    {
        IBaseRepository<News> News { get; }
        IBaseRepository<RssSource> Rss { get; }
        IBaseRepository<Role> Role { get; }
        IBaseRepository<User> Users { get; }
        IBaseRepository<Comment> Comments { get; }
         
        Task<int> SaveChangesAsync();
    }
}