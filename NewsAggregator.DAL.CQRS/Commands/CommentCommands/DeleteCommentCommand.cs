using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Commands.CommentCommands
{
    public class DeleteCommentCommand : IRequest<int>
    {
        public DeleteCommentCommand(CommentDto comment)
        {
            Comment = comment;
        }

        public CommentDto Comment { get; set; }
    }
}