using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;

namespace NewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(NewsAggregatorContext context) : base(context)
        {
        }
        
        //public async Task<User> GetByEmail(string email)
        //{
        //    return await Table.FirstOrDefaultAsync(n => n.Email.Equals(email));
        //}
    }
}