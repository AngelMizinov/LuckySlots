namespace LuckySlots.App.Controllers
{
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
    using LuckySlots.Infrastructure.Enums;

    public class HomeController : Controller
    {
        private readonly IJsonParser parser;
        private readonly IAccountService accountService;
        private readonly UserManager<User> userManager;

        public HomeController(IJsonParser parser, IAccountService accountService, UserManager<User> userManager)
        {
            this.parser = parser;
            this.accountService = accountService;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            this.ViewBag.IsCalledFirstTime = true;
            this.ViewBag.IsValid = true;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ValidationModalViewModel validationModel)
        {
            //CHECK API
            //var result = await this.parser.ExtractExchangeRate("EUR");
            //return Content(result.ToString());

            //GET CURRENT USER
            //var currUser = await this.userManager.GetUserAsync(this.User);

            //JUST TRY DEPOSIT
            //await this.accountService.DepositAsync(currUser.Id, 300, TransactionType.Deposit);

            this.ViewBag.IsCalledFirstTime = false;   
            this.ViewBag.IsValid = true;

            if (!this.ModelState.IsValid)
            {
                this.ViewBag.IsValid = false;
                return View();
            }

            if(validationModel.Year > DateTime.Now.Year - 18)
            {
                this.TempData["Permission"] = "You must be 18 years old to continue.";
                this.ViewBag.IsValid = false;
                return View();
            }
            
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
