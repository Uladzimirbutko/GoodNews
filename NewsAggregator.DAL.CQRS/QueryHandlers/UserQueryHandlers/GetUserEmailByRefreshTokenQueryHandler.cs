using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries.UserQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.UserQueryHandlers
{
    public class GetUserEmailByRefreshTokenQueryHandler :
        IRequestHandler<GetUserEmailByRefreshTokenQuery, string>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;


        public GetUserEmailByRefreshTokenQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<string> Handle(GetUserEmailByRefreshTokenQuery request, CancellationToken cancellationToken)
        {
            var refreshTokenUserId = (await _dbContext.RefreshTokens
                    .AsNoTracking()
                    .FirstOrDefaultAsync(rt => rt.Token.Equals(request.Token),cancellationToken))
                .UserId;

            return (await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id.Equals(refreshTokenUserId),
                cancellationToken: cancellationToken)).Email;
        }
    }
}