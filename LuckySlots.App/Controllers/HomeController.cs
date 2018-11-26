using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LuckySlots.App.Models;
using LuckySlots.Infrastructure.Providers;
using Microsoft.Extensions.Caching.Memory;
using LuckySlots.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using LuckySlots.Data.Models;

namespace LuckySlots.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJsonParser parser;
        private readonly IAccountService accountService;
        private readonly UserManager<User> userManager;
        
        public HomeController(IJsonParser parser,IAccountService accountService,UserManager<User> userManager)
        {
            this.parser = parser;
            this.accountService = accountService;
            this.userManager = userManager;
        }
        
        public async Task<IActionResult> Index()
        {
            //CHECK API
            //var result = await this.parser.ExtractExchangeRate("EUR");
            //return Content(result.ToString());

            //GET CURRENT USER
            //var currUser = await this.userManager.GetUserAsync(this.User);
            
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
