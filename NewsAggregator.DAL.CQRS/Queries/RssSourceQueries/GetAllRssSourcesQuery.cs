using System.Collections.Generic;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.RssSourceQueries
{
    public class GetAllRssSourcesQuery : IRequest<IEnumerable<RssSourceDto>>
    {
        
    }
}