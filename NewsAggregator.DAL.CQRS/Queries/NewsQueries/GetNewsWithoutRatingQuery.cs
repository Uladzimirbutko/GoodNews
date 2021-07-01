using System.Collections.Generic;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.NewsQueries
{
    public class GetNewsWithoutRatingQuery : IRequest<IEnumerable<NewsDto>>
    {
        
    }
}