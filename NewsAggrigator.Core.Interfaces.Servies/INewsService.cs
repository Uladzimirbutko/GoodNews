using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Services.Interfaces
{
    public interface INewsService
    {
        
        Task<IEnumerable<NewsDto>> GetAllNews();
        Task<NewsDto> GetNewsById(Guid id);
        Task<IEnumerable<NewsDto>> GetNewsBySourceId(Guid sourceId);

        Task<IEnumerable<NewsDto>> AggregateNews();
        Task RateNews();
        Task<IEnumerable<NewsDto>> GetNewsWithoutRating();
        Task<int> AddRange(IEnumerable<NewsDto> news);
        Task<int> UpdateNews (IEnumerable<NewsDto> newsDto);
        Task DeleteNews(Guid id);
        
    }
}
