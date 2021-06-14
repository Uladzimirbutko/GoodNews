using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.Core.DataTransferObjects;

namespace NewsAggregator.Core.Services.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsDto>> GetNews(Guid id);
        Task<IEnumerable<NewsDto>> GetNewsBySourceId(Guid? id);
        Task<NewsDto> GetNewsById(Guid id);
        Task<NewsWithRssNameDto> GetNewsWithRssSourceNameById(Guid id);

        Task AddNews(NewsDto news);
        Task AddRange(IEnumerable<NewsDto> news);


        Task<IEnumerable<NewsDto>> GetNewsInfoFromRssSource(RssSourceDto rssSource);


        Task DeleteNews(NewsDto news);
    }
}
