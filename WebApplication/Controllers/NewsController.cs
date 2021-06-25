using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.Services.Interfaces.ServicesInterfaces;
using Serilog;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;


        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("id")]
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

                return Ok(getAll);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return NotFound(e.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var aggregate = await _newsService.AggregateNews();
                stopwatch.Stop();
                Log.Information($"Aggregate time all sources {stopwatch.ElapsedMilliseconds}");

                if (aggregate == null)
                {
                    return BadRequest();
                }
                return Ok();
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return BadRequest();
            }
        }
    }
}
