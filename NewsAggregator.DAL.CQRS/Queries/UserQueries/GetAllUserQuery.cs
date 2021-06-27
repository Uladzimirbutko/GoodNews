using System.Collections.Generic;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.UserQueries
{
    public class GetAllUserQuery : IRequest<IEnumerable<UserDto>>
    {
        
    }
}