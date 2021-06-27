using System;
using System.Collections.Generic;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Queries.CommentQueries
{
    public class GetCommentsByNewsIdQuery : IRequest<IEnumerable<CommentDto>>
    {
        public GetCommentsByNewsIdQuery(Guid newsId)
        {
            NewsId = newsId;
        }

        public Guid NewsId { get; set; }
    }
}