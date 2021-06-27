using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.Services.Implementation.NewsParsers;
using Serilog;

namespace NewsAggregator.Services.Implementation.Services
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IRssSourceService _rssSourceService;

        public NewsService(IUnitOfWork unitOfWork, IMapper mapper, IRssSourceService rssSourceService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _rssSourceService = rssSourceService;
        }

        public async Task<IEnumerable<NewsDto>> AggregateNews()
        {
            try
            {
                var sources = await _rssSourceService.GetAllRssSources();

                var newsUrls = await _unitOfWork.News.GetAll()
                    .Select(n => n.Url)
                    .ToListAsync();

                var newsInfos = new List<NewsDto>();

                var parser = new Parser(_mapper);


                newsInfos.AddRange(parser.Parse(sources, newsUrls));

                await AddRange(newsInfos);

                Log.Information($"Aggregation was succeeded"); 
                
                return newsInfos;
            }
            catch
            {
                Log.Error($"Aggregation was failed");
                return null;
            }

           
        }

        public async Task RateNews()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NewsDto>> GetAllNews()
        {
            return await _unitOfWork.News.GetAll()
                .Select(n => _mapper.Map<NewsDto>(n))
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsDto>> GetNewsBySourceId(Guid id)
        {
            var news = new List<News>();
            try
            {
                news = await _unitOfWork.News
                         .FindBy(n => n.RssSourceId.Equals(id))
                         .ToListAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Error in GetNewsBySourceId. {e.Message}");
            }

            return news
                .Select(n => _mapper.Map<NewsDto>(n))
                .OrderByDescending(dt => dt.PublicationDate);
        }

        public async Task<NewsDto> GetNewsById(Guid id)
        {
            return _mapper.Map<NewsDto>(await _unitOfWork.News.GetById(id));
        }

       
        public async Task<int> AddRange(IEnumerable<NewsDto> news)
        {
            try
            {
                var addRange = news.Select(ent => _mapper.Map<News>(ent));
                Log.Information($"News added {addRange.Count()}");
                await _unitOfWork.News.AddRange(addRange);
                return await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Error in AddRange. {e.Message}");
                return 0;
            }
        }

        public async Task<int> UpdateNews(IEnumerable<NewsDto> news)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteNews(Guid id)
        {
            try
            {
                var oldNews = await _unitOfWork.News.GetById(id);
                await _unitOfWork.News.Remove(oldNews);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Error in Delete. {e.Message}");
            }
        }
    }
}
