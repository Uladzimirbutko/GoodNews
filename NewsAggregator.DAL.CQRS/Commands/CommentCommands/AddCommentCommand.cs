using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Commands.CommentCommands
{
    public class AddCommentCommand : IRequest<int>
    {
        public AddCommentCommand(CommentDto comment)
        {
            Comment = comment;
        }

        public CommentDto Comment { get; set; }
    }
}