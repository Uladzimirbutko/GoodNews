using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class RoleRepository : BaseRepository<Role>
    {
        public RoleRepository(NewsAggregatorContext context) : base(context)
        {
        }
    }
}