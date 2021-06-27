using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.RefreshTokenQuery
{
    public class GetRefreshTokenByTokenValueQuery : IRequest<RefreshTokenDto>
    {
        public GetRefreshTokenByTokenValueQuery(string tokenValue)
        {
            TokenValue = tokenValue;
        }

        public string TokenValue { get; set; }

    }
}