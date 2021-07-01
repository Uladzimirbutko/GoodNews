using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.Models;
using NewsAggregator.Models.ViewModels.News;
using Serilog;

namespace NewsAggregator.Controllers
{
    [Authorize ("18+Content")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ICommentService _commentService;


        public NewsController(INewsService newsService, ICommentService commentService)
        {
            _newsService = newsService;
            _commentService = commentService;
        }

        [Authorize("18+Content")]
        // GET: News
        public async Task<IActionResult> Index(Guid id, int page = 1)
        {
            try
            {
                var news = (await _newsService.GetAllNews())
                    .Where(dto => dto.Rating !=null)
                    .OrderByDescending(dto => dto.Rating)
                    .ToList();

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
                    IsAdmin = HttpContext.User.IsInRole("Admin"),
                    News = newsPerPages,
                    PageInfo = pageInfo
                });
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return View();
            }
        }
        [Authorize("18+Content")]
        // GET: News/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var news = await _newsService.GetNewsById(id);
                var comment = await _commentService.GetCommentsByNewsId(news.Id);

                var viewModel = new NewsWithCommentsViewModel()
                {
                    Id = news.Id,
                    Article = news.Article,
                    Body = news.Body,
                    Url = news.Url,
                    Rating = news.Rating,
                    PublicationDate = news.PublicationDate,
                    Category = news.Category,
                    TitleImage = news.TitleImage,
                    Comments = comment,
                    IsAdmin = HttpContext.User.IsInRole("Admin")
                };
                return View(viewModel);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return View();
            }
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AggregateNews()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AggregateNews(NewsDto news)
        {
            
            await _newsService.AggregateNews();
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRatingToNews(NewsDto news)
        {

            await _newsService.RateNews();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _newsService.DeleteNews(id);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return RedirectToAction("Details");
            }

        }

    }
}
