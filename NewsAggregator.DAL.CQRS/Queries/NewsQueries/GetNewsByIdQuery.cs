using System;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.NewsQueries
{
    public class GetNewsByIdQuery : IRequest<NewsDto>
    {
        public GetNewsByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}