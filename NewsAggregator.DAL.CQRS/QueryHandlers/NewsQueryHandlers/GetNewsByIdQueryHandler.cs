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
    public class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, NewsDto>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetNewsByIdQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<NewsDto> Handle(GetNewsByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<NewsDto>(await _dbContext.News.FirstOrDefaultAsync(source => source.Id.Equals(request.Id), cancellationToken: cancellationToken));
        }
    }
}