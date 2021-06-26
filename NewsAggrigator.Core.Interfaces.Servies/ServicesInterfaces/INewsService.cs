using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Services.Interfaces.ServicesInterfaces
{
    public interface INewsService
    {
        
        Task<IEnumerable<NewsDto>> GetAllNews();
        Task<NewsDto> GetNewsById(Guid id);
        Task<IEnumerable<NewsDto>> GetNewsBySourceId(Guid sourceId);

        Task<IEnumerable<NewsDto>> AggregateNews();
        Task RateNews();
        

        Task<int> AddRange(IEnumerable<NewsDto> news);
        Task<int> UpdateNews (IEnumerable<NewsDto> news);
        Task DeleteNews(Guid id);
        
    }
}
