using System;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.CQRS.Commands.RefreshTokenComands;
using NewsAggregator.DAL.CQRS.Queries.RefreshTokenQuery;

namespace NewsAggregator.Services.Implementation.CqsServices
{
    public class RefreshTokenCqsService : IRefreshTokenService
    {
        private readonly IMediator _mediator;

        public RefreshTokenCqsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RefreshTokenDto> GenerateRefreshToken(Guid userId)
        {
            var newRefreshToken = new RefreshTokenDto
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = Guid.NewGuid().ToString(),
                CreationDate = DateTime.Now.ToLocalTime(),
                ExpiresUtc = DateTime.Now.ToLocalTime().AddHours(1)
            };

            await _mediator.Send(new UpdateCurrentRefreshTokensCommand(userId, newRefreshToken));

            return newRefreshToken;
        }

        public async Task<bool> CheckIsRefreshTokenIsValid(string refreshToken)
        {
            var rt = await _mediator.Send(new GetRefreshTokenByTokenValueQuery(refreshToken));

            return rt != null && rt.ExpiresUtc >= DateTime.Now;
        }
    }
}