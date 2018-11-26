using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LuckySlots.App.Models;
using LuckySlots.Infrastructure.Providers;
using Microsoft.Extensions.Caching.Memory;

namespace LuckySlots.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJsonParser parser;
        private readonly IMemoryCache cache;

        public HomeController(IJsonParser parser, IMemoryCache cache)
        {
            this.parser = parser;
            this.cache = cache;
        }


        public async Task<IActionResult> Index()
        {
            //var result = await this.parser.ExtractExchangeRate("EUR");

            //return Content(result.ToString());
            
            return View();
        }
        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
