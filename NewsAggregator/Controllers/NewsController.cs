using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.Models;
using NewsAggregator.Models.ViewModels.News;
using Serilog;

namespace NewsAggregator.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IRssSourceService _rssSourceService;
        private readonly IWebPageParser _onlinerParser;

        public NewsController(INewsService newsService, IRssSourceService rssSourceService, IWebPageParser onlinerParser)
        {
            _newsService = newsService;
            _rssSourceService = rssSourceService;
            _onlinerParser = onlinerParser;
        }

        // GET: News
        public async Task<IActionResult> Index(Guid id, int page = 1)
        {
            if (id == null)
            {
                return NotFound();
            }
            var news = (await _newsService.GetNews(id)).ToList();

            var pageSize = 10;

            var newsPerPages = news.Skip((page - 1) * pageSize).Take(pageSize);

            var pageInfo = new PageInfo()
            {
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = news.Count
            };

            return View(new NewsListWithPaginationInfo()
            {
                News = newsPerPages,
                PageInfo = pageInfo
            });
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var news = await _newsService.GetNewsWithRssSourceNameById(id);


            if (news == null)
            {
                return NotFound();
            }

            var viewModel = new NewsWithRssNameDto()
            {
                Id = news.Id,
                Article = news.Article,
                Body = news.Body,
                Url = news.Url,
                Rating = news.Rating,
                RssSourceId = news.RssSourceId,
                RssSourceName = news.RssSourceName // Here will Null reference exception -> RssSource is null
            };
            return View(viewModel);
        }

        public async Task<IActionResult> AggregateNews()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AggregateNews(CreateNewsViewModel sources)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var rssSources = await _rssSourceService
                    .GetAllRssSources();

                var newsInfos = new List<NewsDto>(); // without any duplicate

                foreach (var rssSource in rssSources)
                {
                    var newsList = await _newsService.GetNewsInfoFromRssSource(rssSource);

                    if (rssSource.Id.Equals(new Guid("6e6f1eae-80bc-44e2-9831-5b7f7099880a")))
                    {

                        foreach (var newsDto in newsList)
                        {
                            
                            newsDto.Body = await _onlinerParser.Parse(newsDto.Url);
                        } 

                    }
                    newsInfos.AddRange(newsList);
                }

                await _newsService.AddRange(newsInfos);

                stopwatch.Stop();
                Log.Information($"агрегация без паралельного форыча {stopwatch.ElapsedMilliseconds} на {newsInfos.Count} новостей");

            }
            catch (Exception e)
            {
                Log.Error(e, $"{e.Message}");
            }
            return RedirectToAction(nameof(Index));
        }


        // GET: News/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _newsService.GetNewsWithRssSourceNameById(id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(NewsDto news)
        {
            await _newsService.DeleteNews(news);

            return RedirectToAction(nameof(Index));
        }

    }
}
