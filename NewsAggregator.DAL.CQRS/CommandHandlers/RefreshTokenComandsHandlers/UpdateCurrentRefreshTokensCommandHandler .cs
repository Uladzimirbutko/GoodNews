using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands.RefreshTokenComands;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.RefreshTokenComandsHandlers
{
    public class UpdateCurrentRefreshTokensCommandHandler : IRequestHandler<UpdateCurrentRefreshTokensCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateCurrentRefreshTokensCommandHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateCurrentRefreshTokensCommand request, CancellationToken cancellationToken)
        {
            var currentRefreshTokens = await _dbContext.RefreshTokens
                .AsNoTracking()
                .Where(token => token.UserId.Equals(request.UserId))
                .ToListAsync(cancellationToken: cancellationToken);

            _dbContext.RefreshTokens.RemoveRange(currentRefreshTokens);

            await _dbContext.SaveChangesAsync(cancellationToken);

            var rtEntity = _mapper.Map<RefreshToken>(request.NewRefreshToken);

            await _dbContext.AddAsync(rtEntity, cancellationToken);

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
