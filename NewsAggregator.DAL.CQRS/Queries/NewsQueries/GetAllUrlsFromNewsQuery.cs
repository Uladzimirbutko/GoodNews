using System.Collections.Generic;
using MediatR;

namespace NewsAggregator.DAL.CQRS.Queries.NewsQueries
{
    public class GetAllUrlsFromNewsQuery : IRequest<IEnumerable<string>>
    {
        
    }
}