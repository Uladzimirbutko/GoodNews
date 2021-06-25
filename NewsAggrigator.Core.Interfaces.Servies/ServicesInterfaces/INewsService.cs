using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Services.Interfaces.ServicesInterfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDto>> AggregateNews();
        Task<IEnumerable<NewsDto>> GetAllNews();
        Task<IEnumerable<NewsDto>> GetNewsBySourceId(Guid sourceId);
        
        Task<NewsDto> GetNewsById(Guid id);
        
        Task<int> AddRange(IEnumerable<NewsDto> news);
        Task DeleteNews(Guid id);
    }
}
