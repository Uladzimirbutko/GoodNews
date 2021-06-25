using System.Collections.Generic;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Commands.NewsCommands
{
    public class AddRangeNewsCommand : IRequest<int>
    {
        public AddRangeNewsCommand(IEnumerable<NewsDto> news)
        {
            News = news;
        }

        public IEnumerable<NewsDto> News { get; set; }
    }
}