using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsAggregator.Models;
using Serilog;

namespace NewsAggregator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // It's start Test Logger. He writes to the LoggerFile.log system Fatal exception
            //try
            //{
            //    throw new NullReferenceException("Test Exception");
                
            //}
            //catch (Exception e)
            //{
            //    Log.Fatal(e, "Unhanded exception wat throwed  by app");
            //    throw;
            //}
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}