using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries.NewsQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.NewsQueryHandlers
{
    public class GetNewsByRssSourceIdQueryHandler :
        IRequestHandler<GetNewsByRssSourceIdQuery, IEnumerable<NewsDto>>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;


        public GetNewsByRssSourceIdQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewsDto>> Handle(GetNewsByRssSourceIdQuery request, CancellationToken cancellationToken)
        {
            return (await _dbContext.News
                    .Where(n => n.RssSourceId.Equals(request.RssSourceId))
                    .Select(n => _mapper.Map<NewsDto>(n))
                    .ToListAsync(cancellationToken))
                .OrderByDescending(n => n.PublicationDate);
        }
    }
}