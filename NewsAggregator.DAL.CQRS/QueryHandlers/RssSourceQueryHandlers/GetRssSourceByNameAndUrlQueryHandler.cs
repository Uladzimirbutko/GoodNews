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
    public class GetRssSourceByNameAndUrlQueryHandler:
        IRequestHandler<GetRssSourceByNameAndUrlQuery, 
        IEnumerable<RssSourceDto>>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;


        public GetRssSourceByNameAndUrlQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RssSourceDto>> Handle(GetRssSourceByNameAndUrlQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.RssSources
                .Where(source => source.SourceName.Equals(request.Name) ||
                                 source.Url.Equals(request.Url))
                .Select(source => _mapper.Map<RssSourceDto>(source))
                .ToListAsync(cancellationToken);
        }
    }
}