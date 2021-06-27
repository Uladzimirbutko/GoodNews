using System;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Commands.RefreshTokenComands
{
    public class UpdateCurrentRefreshTokensCommand : IRequest<int>
    {
        public UpdateCurrentRefreshTokensCommand(Guid userId, RefreshTokenDto newRefreshToken)
        {
            UserId = userId;
            NewRefreshToken = newRefreshToken;
        }

        public Guid UserId { get; set; }

        public RefreshTokenDto NewRefreshToken { get; set; }

    }
}