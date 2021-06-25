using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.Core.Services.Interfaces;

namespace NewsAggregator.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly ICommentService _commentService;


        public NewsController(INewsService newsService, ICommentService commentService)
        {
            _newsService = newsService;
            _commentService = commentService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var news = await _newsService.GetNewsById(id);

            return Ok(news);
        }
    }
}
