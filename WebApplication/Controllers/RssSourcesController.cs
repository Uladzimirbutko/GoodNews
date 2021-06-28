using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using NewsAggregator.Core.Services.Interfaces;
using Serilog;

namespace WebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RssSourcesController : ControllerBase
    {
        private readonly IRssSourceService _rssSourceService;

        public RssSourcesController(IRssSourceService rssSourceService)
        {
            _rssSourceService = rssSourceService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var news = await _rssSourceService.GetRssSourceById(id);

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
                var getAll = await _rssSourceService.GetAllRssSources();

                return Ok(getAll);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return NotFound(e.Message);
            }
        }
    }
}
