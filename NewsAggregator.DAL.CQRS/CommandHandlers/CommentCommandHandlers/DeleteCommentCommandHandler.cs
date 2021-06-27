using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands.CommentCommands;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.CommentCommandHandlers
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public DeleteCommentCommandHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
             _dbContext.Comments.Remove(_mapper.Map<Comment>(request.Comment));

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}