using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries.NewsQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.NewsQueryHandlers
{
    public class GetAllUrlsFromNewsQueryHandler : 
        IRequestHandler<GetAllUrlsFromNewsQuery, IEnumerable<string>>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;


        public GetAllUrlsFromNewsQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<string>> Handle(GetAllUrlsFromNewsQuery request, CancellationToken cancellationToken)
        {
            var urls = await _dbContext.News
                .Select(news => news.Url)
                .ToListAsync(cancellationToken);
            return urls;
        }
    }
}