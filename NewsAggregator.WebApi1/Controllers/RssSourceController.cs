using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces.ServicesInterfaces;

namespace NewsAggregator.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RssSourceController : ControllerBase
    {
        private readonly IRssSourceService _rssSourceService;

        public RssSourceController(IRssSourceService rssSourceService)
        {
            _rssSourceService = rssSourceService;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var source = await _rssSourceService.GetRssSourceById(id);

            return Ok(source);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var sources = await _rssSourceService.GetAllRssSources();
            //if (!string.IsNullOrEmpty(name))
            //{
            //    sources = sources.Where(dto => dto.SourceName.Contains(name));
            //}
            //if (!string.IsNullOrEmpty(url))
            //{
            //    sources = sources.Where(dto => dto.Url.Contains(url));
            //}

            return Ok(sources);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RssSourceDto request)
        {
            var sources = await _rssSourceService.GetAllRssSources();

            return Ok(sources);
        }
        //[HttpPut]
        //public async Task<IActionResult> Put()
        //{
        //    var sources = await _rssSourceService.GetAllRssSources();

        //    return Ok(sources);
        //}
        [HttpPatch]
        public async Task<IActionResult> Patch()
        {
            var sources = await _rssSourceService.GetAllRssSources();

            return Ok(sources);
        }
    }
}
