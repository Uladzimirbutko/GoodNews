using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Core
{
    public class NewsAggregatorContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RssSource> RssSources { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public NewsAggregatorContext(DbContextOptions<NewsAggregatorContext> options) 
            :base(options)
        {
            
        }
    }
}