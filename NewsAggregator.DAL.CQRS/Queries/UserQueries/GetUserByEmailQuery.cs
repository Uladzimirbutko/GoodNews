using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.UserQueries
{
    public class GetUserByEmailQuery: IRequest<UserDto>
    {
        public string Email{ get; set; }

        public GetUserByEmailQuery(string email)
        {
            Email = email;
        }

    }
}