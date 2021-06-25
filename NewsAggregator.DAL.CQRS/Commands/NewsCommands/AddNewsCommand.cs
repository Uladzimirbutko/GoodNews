using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Commands.NewsCommands
{
    public class AddNewsCommand : IRequest<int>
    {
        public AddNewsCommand(NewsDto news)
        {
            News = news;
        }

        public NewsDto News { get; set; }
    }
}