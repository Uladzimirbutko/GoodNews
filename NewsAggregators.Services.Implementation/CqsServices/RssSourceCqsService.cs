using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Interfaces.Services;
using NewsAggregator.DAL.CQRS.Queries.RssSourceQueries;

namespace NewsAggregator.Services.Implementation.CqsServices
{
    public class RssSourceCqsService : IRssSourceService
    {
        private readonly IMediator _mediator;


        public RssSourceCqsService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IEnumerable<RssSourceDto>> GetRssSourcesByNameAndUrl
            (string name, string url)
        {
            var getRssSourceByNameAndUrlQueryHandler = new GetRssSourceByNameAndUrlQuery(name, url);
            return await _mediator.Send(getRssSourceByNameAndUrlQueryHandler);
        }

        public async Task<IEnumerable<RssSourceDto>> GetAllRssSources()
        {
            var query = new GetAllRssSourcesQuery();
            return await _mediator.Send(query);
        }

        public async Task<RssSourceDto> GetRssSourceById(Guid id)
        {
            var getRssSourceByIdQuery = new GetRssSourceByIdQuery(id);
            var result = await _mediator.Send(getRssSourceByIdQuery);
            return result;
        }

    }
}