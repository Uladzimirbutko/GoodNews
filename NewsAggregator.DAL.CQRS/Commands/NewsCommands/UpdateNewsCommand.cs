using System.Collections.Generic;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.DAL.CQRS.Commands.NewsCommands
{
    public class UpdateNewsCommand : IRequest<int>
    {
        public UpdateNewsCommand(IEnumerable<NewsDto> newsDtos)
        {
            NewsDtos = newsDtos;
        }

        public IEnumerable<NewsDto> NewsDtos { get; set; }
    }
}