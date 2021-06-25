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
    public class GetRssSourceByIdQueryHandler : IRequestHandler<GetRssSourceByIdQuery, RssSourceDto>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;


        public GetRssSourceByIdQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<RssSourceDto> Handle(GetRssSourceByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<RssSourceDto>(await _dbContext.RssSources.FirstOrDefaultAsync(source => source.Id.Equals(request.Id), cancellationToken: cancellationToken));
        }
    }
}