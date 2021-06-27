using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Commands.CommentCommands
{
    public class EditCommentCommand : IRequest<int>
    {
        public EditCommentCommand(CommentDto comment)
        {
            Comment = comment;
        }

        public CommentDto Comment { get; set; }
    }
}