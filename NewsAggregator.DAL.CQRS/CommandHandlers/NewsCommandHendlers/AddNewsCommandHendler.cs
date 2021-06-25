using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands.NewsCommands;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.NewsCommandHendlers
{
    public class AddNewsCommandHandler :IRequestHandler<AddNewsCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddNewsCommandHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddNewsCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.News.AddAsync(_mapper.Map<News>(request), cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}