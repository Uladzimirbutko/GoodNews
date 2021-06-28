using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries.UserQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.UserQueryHandlers
{
    public class CheckAuthenticationQueryHandler :
        IRequestHandler<CheckAuthenticationQuery, bool>
    {
        private readonly NewsAggregatorContext _dbContext;

        public CheckAuthenticationQueryHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(CheckAuthenticationQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Email.Equals(request.Email) 
                               && u.PasswordHash.Equals(request.PasswordHash), cancellationToken);
        }
    }
}