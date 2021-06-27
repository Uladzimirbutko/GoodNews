using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;
using NewsAggregator.Models;
using NewsAggregator.Models.ViewModels.News;

namespace NewsAggregator.Controllers
{
    [Authorize ("18+Content")]
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        private readonly IRssSourceService _rssSourceService;


        public NewsController(INewsService newsService, ICommentService commentService, IUserService userService, IRssSourceService rssSourceService)
        {
            _newsService = newsService;
            _commentService = commentService;
            _userService = userService;
            _rssSourceService = rssSourceService;
        }
        [Authorize("18+Content")]
        // GET: News
        public async Task<IActionResult> Index(Guid id, int page = 1)
        {
            var news = (await _newsService.GetAllNews()).OrderByDescending(dto => dto.PublicationDate).ToList();

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
        [Authorize("18+Content")]
        // GET: News/Details/5
        public async Task<IActionResult> Details(Guid id)
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
            };
            return View(viewModel);
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

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
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
        public async Task<IActionResult> DeleteConfirmed(NewsViewModel model)   
        {
            await _newsService.DeleteNews(model.Id);

            return RedirectToAction(nameof(Index));
        }

    }
}
