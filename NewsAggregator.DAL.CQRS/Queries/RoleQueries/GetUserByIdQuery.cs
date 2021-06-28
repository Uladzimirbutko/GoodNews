using System;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.RoleQueries
{
    public class GetRoleByUserIdQuery : IRequest<RoleDto>
    {
        public GetRoleByUserIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}