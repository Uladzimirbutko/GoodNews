using System;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Commands.RoleCommand
{
    public class ReplaceUserRoleCommand : IRequest<int>
    {
        public ReplaceUserRoleCommand(Guid userId, RoleDto role)
        {
            UserId = userId;
            Role = role;
        }

        public Guid UserId  { get; set; }
        public RoleDto Role { get; set; }
    }
}