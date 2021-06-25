using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries.RssSourceQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.RssSourceQueryHandlers
{
    public class GetAllRssSourcesQueryHandler :
        IRequestHandler<GetAllRssSourcesQuery, IEnumerable<RssSourceDto>>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;


        public GetAllRssSourcesQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RssSourceDto>> Handle(GetAllRssSourcesQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.RssSources.Select(dto => _mapper.Map<RssSourceDto>(dto)).ToListAsync(cancellationToken: cancellationToken);
        }
    }
}