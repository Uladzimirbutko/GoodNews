using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries.RefreshTokenQuery;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.RefreshTokenQueryHandlers
{
    public class GetRefreshTokenByTokenValueQueryHandler : IRequestHandler<GetRefreshTokenByTokenValueQuery, RefreshTokenDto>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetRefreshTokenByTokenValueQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<RefreshTokenDto> Handle(GetRefreshTokenByTokenValueQuery request, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<RefreshTokenDto>(await _dbContext.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.Token.Equals(request.TokenValue), cancellationToken));

            return result;
        }

    }
}