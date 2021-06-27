using MediatR;

namespace NewsAggregator.DAL.CQRS.Queries.UserQueries
{
    public class GetUserRoleNameByEmailQuery : IRequest<string>
    {
        public GetUserRoleNameByEmailQuery(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
    }
}