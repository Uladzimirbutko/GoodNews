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
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, CommentDto>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetCommentByIdQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<CommentDto> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<CommentDto>(await _dbContext.Comments.AsNoTracking().FirstOrDefaultAsync(comment => comment.Id.Equals(request.Id),  cancellationToken));
        }
    }
}