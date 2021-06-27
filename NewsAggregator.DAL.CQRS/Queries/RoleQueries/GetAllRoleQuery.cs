using System.Collections.Generic;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.RoleQueries
{
    public class GetAllRoleQuery : IRequest<IEnumerable<RoleDto>>
    {
        
    }
}