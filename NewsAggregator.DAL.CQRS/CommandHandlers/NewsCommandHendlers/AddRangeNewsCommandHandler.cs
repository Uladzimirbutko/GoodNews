using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands.NewsCommands;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.NewsCommandHendlers
{
    public class AddRangeNewsCommandHandler : IRequestHandler<AddRangeNewsCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddRangeNewsCommandHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddRangeNewsCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.News.AddRangeAsync(request.News
                .Select(news => _mapper.Map<News>(news)), cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}