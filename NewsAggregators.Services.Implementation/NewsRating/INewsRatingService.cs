using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Services.Implementation.NewsRating
{
    public interface INewsRatingService
    {
        Task<IEnumerable<NewsDto>> Rating(IEnumerable<NewsDto> newsWithoutRating);
    }
}