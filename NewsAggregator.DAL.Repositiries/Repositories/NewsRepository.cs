using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class NewsRepository : BaseRepository<News>
    {
        public NewsRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}