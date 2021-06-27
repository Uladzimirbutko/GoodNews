using MediatR;

namespace NewsAggregator.DAL.CQRS.Queries.UserQueries
{
    public class CheckAuthenticationQuery : IRequest<bool>
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public CheckAuthenticationQuery(string email, string passwordHash)
        {
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}