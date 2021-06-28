using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.CQRS.Commands.RoleCommand;
using NewsAggregator.DAL.CQRS.Queries.RoleQueries;
using Serilog;

namespace NewsAggregator.Services.Implementation.CqsServices
{
    public class RoleCqsService : IRoleService
    {
        private readonly IMediator _mediator;

        public RoleCqsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<RoleDto>> GetRoles()
        {
            try
            {
                return await _mediator.Send(new GetAllRoleQuery());
            }
            catch (Exception e)
            {
                Log.Error($"Error Get All Role {e.Message}");
                return null;
            }
        }

        public async Task<RoleDto> GetRoleByUserId(Guid userId)
        {
            try
            {
                return await _mediator.Send(new GetRoleByUserIdQuery(userId));
            }
            catch (Exception e)
            {
                Log.Error($"Error Get Role By User Id {e.Message}. {userId}");
                return null;
            }
        }

        public async Task ReplaceUserRole(Guid userId, RoleDto role)
        {
            try
            {
               var replace= await _mediator.Send(new ReplaceUserRoleCommand(userId, role));
               Log.Information($"Replace User role succeeded {userId}. {role.Name} ");
            }
            catch (Exception e)
            {
                Log.Error($"Error Replace User Role {e.Message} . {userId}");
            }
        }
    }
}