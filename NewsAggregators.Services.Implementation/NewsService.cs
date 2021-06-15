using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using NewsAggregator.DAL.Repositories.Implementation;
using NewsAggregator.Services.Implementation.NewsParsers;
using Serilog;

namespace NewsAggregator.Services.Implementation
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
            var sources = await _rssSourceService.GetAllRssSources();

            var newsUrls = await _unitOfWork.News.GetAllNews()
                .Select(n => n.Url)
                .ToListAsync();

            var newsInfos = new List<NewsDto>();

            var parser = new Parser(_mapper);

            try
            {
                newsInfos.AddRange(parser.Parse(sources, newsUrls));

                await AddRange(newsInfos);

                Log.Information($"Aggregation was succeeded");
            }
            catch
            {
                Log.Error($"Aggregation was failed");
            }

            return newsInfos;
        }

        public async Task<IEnumerable<NewsDto>> GetAllNews()
        {
            return await _unitOfWork.News
                .FindBy(n => n.Id.Equals(n.Id))
                .Select(n => _mapper.Map<NewsDto>(n))
                .ToListAsync();
        }

        public async Task<IEnumerable<NewsDto>> GetNewsBySourceId(Guid? id)
        {
            var news = new List<News>();
            try
            {
                news = id.HasValue
                    ? await _unitOfWork.News
                        .FindBy(n
                            => n.RssSourceId.Equals(id.GetValueOrDefault()))
                        .ToListAsync()
                    : await _unitOfWork.News
                        .FindBy(n => n.Id != null)
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

        public async Task AddNews(NewsDto news)
        {
            try
            {
                await _unitOfWork.News.Add(_mapper.Map<News>(news));
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Error in Add one News. {e.Message}");
            }
        }

        public async Task AddRange(IEnumerable<NewsDto> news)
        {
            try
            {
                var addRange = news.Select(ent => _mapper.Map<News>(ent));
                await _unitOfWork.News.AddRange(addRange);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log.Error($"Error in AddRange. {e.Message}");
            }
        }

        public async Task DeleteNews(NewsDto news)
        {
            try
            {
                var oldNews = await _unitOfWork.News.GetById(news.Id);
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
