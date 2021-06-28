using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Commands.NewsCommands;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.NewsCommandHendlers
{
    public class UpdateNewsCommandHandler : IRequestHandler<UpdateNewsCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;

        public UpdateNewsCommandHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(UpdateNewsCommand request, CancellationToken cancellationToken)
        {

            foreach (var dto in request.NewsDtos)
            {
               var news = await _dbContext.News.FirstOrDefaultAsync(newsId => newsId.Id.Equals(dto.Id), cancellationToken);
               news.Rating = dto.Rating; 
            }
            
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}