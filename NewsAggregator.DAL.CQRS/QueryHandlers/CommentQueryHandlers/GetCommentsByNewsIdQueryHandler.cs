using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries.CommentQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.CommentQueryHandlers
{
    public class GetCommentsByNewsIdQueryHandler : IRequestHandler<GetCommentsByNewsIdQuery, IEnumerable<CommentDto>>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetCommentsByNewsIdQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentDto>> Handle(GetCommentsByNewsIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Comments
                .Where(comment => comment.NewsId.Equals(request.NewsId))
                .Select(comment => _mapper.Map<CommentDto>(comment))
                .ToListAsync(cancellationToken: cancellationToken);

            return result;
        }
    }
}