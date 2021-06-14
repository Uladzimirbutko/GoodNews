using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class RssSourceRepository : BaseRepository<RssSource>
    {
        public RssSourceRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}