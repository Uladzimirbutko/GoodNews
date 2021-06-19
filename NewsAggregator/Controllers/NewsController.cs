using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.Models;
using NewsAggregator.Models.ViewModels.News;
using NewsAggregator.Services.Implementation.NewsParsers;
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
            var news = (await _newsService.GetAllNews()).ToList();

            var pageSize = 12;

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
            var news = await _newsService.GetNewsById(id);


            if (news == null)
            {
                return NotFound();
            }

            var viewModel = new NewsDto()
            {
                Id = news.Id,
                Article = news.Article,
                Body = news.Body,
                Url = news.Url,
                Rating = news.Rating,
                RssSourceId = news.RssSourceId,
                PublicationDate = news.PublicationDate, // Here will Null reference exception -> RssSource is null
                Summary = news.Summary,
                Category = news.Category,
                TitleImage = news.TitleImage
            };
            return View(viewModel);
        }

        public async Task<IActionResult> AggregateNews()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AggregateNews(NewsDto news)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await _newsService.AggregateNews();
            stopwatch.Stop();
            Log.Information($"Время аггрегации онлайнера {stopwatch.ElapsedMilliseconds}");
            return RedirectToAction(nameof(Index));
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _newsService.GetNewsById(id);
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
