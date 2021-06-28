using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Commands.CommentCommands;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.CommentCommandHandlers
{
    public class EditCommentCommandHandler : IRequestHandler<EditCommentCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public EditCommentCommandHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(EditCommentCommand request, CancellationToken cancellationToken)
        {
            //var comment = await _dbContext.Comments
            //    .FirstOrDefaultAsync(comment => comment.Id.Equals(request.Comment.Id), cancellationToken: cancellationToken);
            // comment = _mapper.Map<Comment>(request.Comment);

            var commentEnt = await _dbContext.Comments.FirstOrDefaultAsync(comment => comment.Id.Equals(request.Comment.Id), cancellationToken: cancellationToken);
            commentEnt.Text = request.Comment.Text;

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}