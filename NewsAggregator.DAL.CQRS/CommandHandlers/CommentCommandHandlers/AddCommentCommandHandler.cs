using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands.CommentCommands;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.CommentCommandHandlers
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddCommentCommandHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Comments.AddAsync(_mapper.Map<Comment>(request.Comment), cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}