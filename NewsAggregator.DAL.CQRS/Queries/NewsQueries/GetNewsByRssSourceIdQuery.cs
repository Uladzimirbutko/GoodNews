using System;
using System.Collections.Generic;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.NewsQueries
{
    public class GetNewsByRssSourceIdQuery : IRequest<IEnumerable<NewsDto>>
    {
        public GetNewsByRssSourceIdQuery(Guid id)
        {
            RssSourceId = id;
        }

        internal Guid RssSourceId { get; set; }
    }
}