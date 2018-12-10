namespace LuckySlots.App.Controllers
{
    using LuckySlots.App.Models;
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Providers;
    using LuckySlots.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly IJsonParser parser;
        private readonly UserManager<User> userManager;
        private readonly ICreditCardService creditCardService;
        private readonly ITransactionServices transactionServices;
        private readonly IUserManagementServices userManagementServices;

        public HomeController(IJsonParser parser, UserManager<User> userManager, ICreditCardService creditCardService,
            ITransactionServices transactionServices, IUserManagementServices userManagementServices)
        {
            this.parser = parser;
            this.userManager = userManager;
            this.creditCardService = creditCardService;
            this.transactionServices = transactionServices;
            this.userManagementServices = userManagementServices;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // TODO: Consider refactoring
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

            if (validationModel.Year > DateTime.Now.Year - 18)
            {
                this.TempData["Permission"] = "You must be 18 years old to continue.";
                this.ViewBag.IsValid = false;
                return View();
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Deposit()
        {
            ViewData["Message"] = "Your application deposit page.";

            return RedirectToAction(nameof(DepositSuccessful));
        }

        public IActionResult DepositSuccessful()
        {
            return RedirectToAction(nameof(Deposit));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Deposit(DepositViewModel model)
        {
            ViewData["Message"] = "Your application deposit page.";

            return View();
        }

        public IActionResult About()
        {
            if (this.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
            {
                return PartialView();
            }

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            if (this.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
            {
                return PartialView();
            }

            return View();
        }

        public IActionResult Privacy()
        {
            if (this.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
            {
                return PartialView();
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
