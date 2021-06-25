using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Commands.NewsCommands;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.NewsCommandHendlers
{
    public class DeleteNewsCommandHandler : IRequestHandler<DeleteNewsCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;

        public DeleteNewsCommandHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(DeleteNewsCommand request, CancellationToken cancellationToken)
        {
            _dbContext.News.Remove(await _dbContext.News.FirstOrDefaultAsync(news =>
                news.Id.Equals(request.NewsId), cancellationToken));

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}