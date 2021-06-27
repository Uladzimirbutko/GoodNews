using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.CQRS.Commands.NewsCommands;
using NewsAggregator.DAL.CQRS.Queries.NewsQueries;
using NewsAggregator.Services.Implementation.NewsParsers;
using NewsAggregator.Services.Implementation.NewsRating;
using Serilog;

namespace NewsAggregator.Services.Implementation.CqsServices
{
    public class NewsCqsService : INewsService
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IRssSourceService _rssSourceService;
        private readonly INewsRatingService _newsRatingService;



        public NewsCqsService(IMapper mapper, IMediator mediator, IRssSourceService rssSourceService, INewsRatingService newsRatingService)
        {
            _mapper = mapper;
            _mediator = mediator;
            _rssSourceService = rssSourceService;
            _newsRatingService = newsRatingService;
        }


        public async Task<IEnumerable<NewsDto>> GetNewsBySourceId(Guid sourceId)
        {
            try
            {
                return await _mediator.Send(new GetNewsByRssSourceIdQuery(sourceId));
            }
            catch (Exception e)
            {
                Log.Error($"Exception {e.Message}");
                return null;
            }
        }


        public async Task<NewsDto> GetNewsById(Guid id)
        {
            try
            {
                return await _mediator.Send(new GetNewsByIdQuery(id));
            }
            catch (Exception e)
            {
                Log.Error($"Exception {e.Message}");
                return null;
            }
        }


        public async Task<int> AddRange(IEnumerable<NewsDto> news)
        {
            try
            {
                var result = await _mediator.Send(new AddRangeNewsCommand(news));
                Log.Information($"Added {result} News");
                return result;
            }
            catch (Exception e)
            {
                Log.Error($"AddRangeNews Exception: {e.Message}");
                return 0;

            }
        }

        public async Task<int> UpdateNews(IEnumerable<NewsDto> news)
        {
            try
            {
                var count = await _mediator.Send(new UpdateNewsCommand(news));
                Log.Information($"Updaded {count} News");
                return count;
            }
            catch (Exception e)
            {
                Log.Error($"Error in Update {e.Message}");
                return default;
            }

        }

        public async Task DeleteNews(Guid id)
        {
            try
            {
                await _mediator.Send(new DeleteNewsCommand(id));
            }
            catch (Exception e)
            {
                Log.Error($"DeleteNews Exception: {e.Message}");
            }
        }

        public async Task<IEnumerable<string>> GetAllUrlsFromNews()
        {
            try
            {
                return await _mediator.Send(new GetAllUrlsFromNewsQuery());
            }
            catch (Exception e)
            {
                Log.Error($"GetAllNews Exception {e.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<NewsDto>> AggregateNews()
        {
            try
            {
                var sources = await _rssSourceService.GetAllRssSources();

                var urls = await GetAllUrlsFromNews();

                var newsInfos = new List<NewsDto>();

                var parser = new Parser(_mapper);

                newsInfos.AddRange(parser.Parse(sources, urls));

                await AddRange(newsInfos);

                Log.Information($"Aggregation was succeeded");

                return newsInfos;
            }
            catch (Exception e)
            {
                Log.Error($"Aggregation was failed{e.Message}");
                return null;
            }

        }

        public async Task<IEnumerable<NewsDto>> GetAllNews()
        {
            try
            {
                return await _mediator.Send(new GetAllNewsQuery());
            }
            catch (Exception e)
            {
                Log.Error($"GetAllNews Exception {e.Message}");
                return null;
            }
        }

        public async Task RateNews()
        {
            var getAllNewsWithoutRatingQuery = await _mediator.Send(new GetAllNewsWithoutRatingQuery());


            if (getAllNewsWithoutRatingQuery != null)
            {
                var newsWithRating = await _newsRatingService.Rating(getAllNewsWithoutRatingQuery);

                await UpdateNews(newsWithRating);
            }


        }

    }

}


