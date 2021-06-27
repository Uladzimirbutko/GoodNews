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
    public class GetAllNewsWithoutRatingQueryHandler :
        IRequestHandler<GetAllNewsWithoutRatingQuery, IEnumerable<NewsDto>>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;


        public GetAllNewsWithoutRatingQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewsDto>> Handle(GetAllNewsWithoutRatingQuery request, CancellationToken cancellationToken)
        {
            var news = await _dbContext.News
                .Where(rt => rt.Rating == null)
                .Take(29)
                .Select(dto => _mapper.Map<NewsDto>(dto))
                .ToListAsync(cancellationToken);
            return news;
        }
    }
}