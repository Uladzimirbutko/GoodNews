//using Microsoft.EntityFrameworkCore;
//using NewsAggregator.Core.DataTransferObjects;
//using NewsAggregator.Core.Services.Interfaces;
//using NewsAggregator.DAL.Core.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Xml;
//using AutoMapper;
//using NewsAggregator.DAL.Repositories.Implementation;
//using System.ServiceModel.Syndication;

//namespace NewsAggregator.Services.Implementation
//{
//    public class NewsService1
//    {
//        private readonly IUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;
//        private readonly IRssSourceService _rssSourceService;

//        public NewsService1(IUnitOfWork unitOfWork, IMapper mapper, IRssSourceService rssSourceService)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//            _rssSourceService = rssSourceService;
//        }

        

//        public async Task<NewsDto> GetNewsById(Guid id)
//        {
            
//            return _mapper.Map<NewsDto>(await _unitOfWork.News.GetById(id));
//        }

//        public IEnumerable<NewsDto> Search(IEnumerable<NewsDto> news, string expression)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<NewsWithRssNameDto> GetNewsWithRssSourceNameById(Guid id)
//        {
//            var result = await _unitOfWork.News
//                .FindBy(n => n.RssSourceId.Equals(id),
//                    n => n.RssSource)
//                .Select(n => new NewsWithRssNameDto()
//                {
//                    Id = n.Id,
//                    Article = n.Article,
//                    Url = n.Url,
//                    Body = n.Body,
//                    Summary = n.Summary,
//                    PublicationDate = n.PublicationDate,
//                    Rating = n.Rating,
//                    RssSourceName = n.RssSource.SourceName,
//                    RssSourceId = n.RssSource.Id

//                }).FirstOrDefaultAsync();

//            return result;
//        }

//        public async Task AddRange(IEnumerable<NewsDto> news)
//        {
//            var addRange = news.Select(ent => _mapper.Map<News>(ent)).ToList();

//            await _unitOfWork.News.AddRange(addRange);
//            await _unitOfWork.SaveChangesAsync();
//        }

        

//        public async Task<IEnumerable<NewsDto>> AggregateNewsFromRss()
//        {
//            return null;
//        }

//        public async Task<IEnumerable<NewsDto>> GetNewsInfoFromRssSource(RssSourceDto rssSource) // pars rss source 
//        {
//            var news = new List<NewsDto>();
//            using (var reader = XmlReader.Create(rssSource.Url))
//            {
//                var feed = SyndicationFeed.Load(reader);
//                reader.Close();
//                if (feed.Items.Any())
//                {
//                    var currentNewsUrls = await _unitOfWork.News.GetAllNews()
//                         .Select(n => n.Url)
//                         .ToListAsync();

//                    foreach (var syndicationItem in feed.Items)
//                    {
//                        if (!currentNewsUrls.Any(url => url.Equals(syndicationItem.Id)))
//                        {
//                            var newsDto = new NewsDto()
//                            {
//                                Id = Guid.NewGuid(),
//                                RssSourceId = rssSource.Id,
//                                Url = syndicationItem.Id,
//                                Article = syndicationItem.Title.Text,
//                                Summary = syndicationItem.Summary.Text,
//                                PublicationDate = syndicationItem.PublishDate
//                                    .LocalDateTime
//                            };
//                            news.Add(newsDto);
//                        }

//                    }
//                }

//            }

//            return news;
//        }

//        public async Task DeleteNews(NewsDto news)
//        {
//            var oldNews = await _unitOfWork.News.GetById(news.Id);
//            await _unitOfWork.News.Remove(oldNews);
//            await _unitOfWork.SaveChangesAsync();

//        }



//    }
//}
