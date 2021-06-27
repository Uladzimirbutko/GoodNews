using System;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.CommentQueries
{
    public class GetCommentByIdQuery : IRequest<CommentDto>
    {
        public GetCommentByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}