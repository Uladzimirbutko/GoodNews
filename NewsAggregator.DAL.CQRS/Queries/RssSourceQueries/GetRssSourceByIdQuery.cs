using System;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.RssSourceQueries
{
    public class GetRssSourceByIdQuery: IRequest<RssSourceDto>
    {
        public Guid Id{ get; set; }

        public GetRssSourceByIdQuery(Guid id)
        {
            Id = id;
        }

    }
}