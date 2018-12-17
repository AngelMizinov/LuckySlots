namespace LuckySlots.App.Controllers
{
    using LuckySlots.App.Models;
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Providers;
    using LuckySlots.Services.Contracts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Diagnostics;

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
            this.ViewBag.IsValid = false;

            return View("Index");
        }

        [HttpPost]
        public IActionResult Index(ValidationModalViewModel validationModel)
        {
            this.ViewBag.IsValid = true;

            if (!this.ModelState.IsValid)
            {
                this.TempData["Permission"] = "You must be 18 years old to continue.";
                this.ViewBag.IsValid = false;
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Chat()
        {
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
