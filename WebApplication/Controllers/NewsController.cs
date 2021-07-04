using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.Interfaces.Services;
using Serilog;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;


        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var news = await _newsService.GetNewsById(id);

                if (news == null)
                {
                    return NotFound();
                }

                return Ok(news);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return NotFound();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var getAll = await _newsService.GetAllNews();

                return Ok(getAll.Take(5).Where(dto => dto.Rating != null));
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return NotFound(e.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _newsService.DeleteNews(id);

                return Ok("News deleted");
            }
            catch
            {
                return StatusCode(400);
            }
        }
    }
}
