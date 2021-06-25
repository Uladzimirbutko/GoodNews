using System;
using MediatR;

namespace NewsAggregator.DAL.CQRS.Commands.NewsCommands
{
    public class DeleteNewsCommand : IRequest<int>
    {
        public DeleteNewsCommand(Guid newsId)
        {
            NewsId = newsId;
        }

        public Guid NewsId { get; set; }
    }
}