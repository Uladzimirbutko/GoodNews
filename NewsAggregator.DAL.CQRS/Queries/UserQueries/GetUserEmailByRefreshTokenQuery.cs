using MediatR;

namespace NewsAggregator.DAL.CQRS.Queries.UserQueries
{
    public class GetUserEmailByRefreshTokenQuery : IRequest<string>
    {
        public string Token{ get; set; }

        public GetUserEmailByRefreshTokenQuery(string token)
        {
            Token = token;
        }

    }
}