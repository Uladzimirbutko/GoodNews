using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Commands.RoleCommand;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.RoleCommandHandlers
{
    public class ReplaceUserRoleCommandHandler : IRequestHandler<ReplaceUserRoleCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;

        public ReplaceUserRoleCommandHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(ReplaceUserRoleCommand request, CancellationToken cancellationToken)
        {
            var roleId = request.Role.Id;
            var user = await _dbContext.Users.FirstOrDefaultAsync(user1 => user1.Id.Equals(request.UserId),
                cancellationToken);
            user.RoleId = roleId;
           return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}