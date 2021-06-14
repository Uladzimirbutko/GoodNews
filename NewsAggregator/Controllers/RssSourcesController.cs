using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.Core.DataTransferObjects;
using NewsAggregator.Core.Services.Interfaces;

namespace NewsAggregator.Controllers
{
    public class RssSourcesController : Controller
    {
        private readonly IRssSourceService _rssSource;

        public RssSourcesController(IRssSourceService rssSource)
        {
            _rssSource = rssSource;
        }

        // GET: RssSources
        public async Task<IActionResult> Index()
        {
            
            var model = await _rssSource.GetAllRssSources();
            return View(model);
        }

        // GET: RssSources/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rssSource = await _rssSource.GetRssSourceById(id.Value);
            if (rssSource == null)
            {
                return NotFound();
            }

            return View(rssSource);
        }

        // GET: RssSources/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rssSource = await _rssSource.GetRssSourceById(id.Value);
            if (rssSource == null)
            {
                return NotFound();
            }

            return View(rssSource);
        }

        // POST: RssSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var findRss = await _rssSource.GetRssSourceById(id);
            await _rssSource.DeleteRssSource(findRss);
            return RedirectToAction(nameof(Index));
        }

    }
}
