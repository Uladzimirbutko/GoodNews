using System;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.UserQueries
{
    public class GetUserByIdQuery: IRequest<UserDto>
    {
        public Guid Id{ get; set; }

        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }

    }
}