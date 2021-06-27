using System.Linq;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries.UserQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.UserQueryHandlers
{
    public class GetUserRoleNameByEmailQueryHandler : IRequestHandler<GetUserRoleNameByEmailQuery, string>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserRoleNameByEmailQueryHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> Handle(GetUserRoleNameByEmailQuery request, CancellationToken cancellationToken)
        {
            var userRoleId = _dbContext.Users.AsNoTracking()
                .FirstOrDefaultAsync(user => user.Email.Equals(request.Email), cancellationToken: cancellationToken).Result.RoleId;
            var result =  _dbContext.Roles.AsNoTracking().FirstOrDefault(role => role.Id.Equals(userRoleId)).Name;
            return result;
        }
    }
}