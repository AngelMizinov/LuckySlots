namespace LuckySlots.App.Controllers
{
    using LuckySlots.App.Models;
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Enums;
    using LuckySlots.Services.Contracts;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ICreditCardService creditCardService;
        private readonly IAccountService accountService;

        public AccountController(UserManager<User> userManager, ICreditCardService creditCardService,
            IAccountService accountService)
        {
            this.userManager = userManager;
            this.creditCardService = creditCardService;
            this.accountService = accountService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Deposit()
        {
            var currUser = await this.userManager.GetUserAsync(this.User);

            var cards = await this.creditCardService.GetAllByUserIdAsync(currUser.Id);

            DepositViewModel model = new DepositViewModel()
            {
                CreditCards = cards.Select(c => new SelectListItem
                {
                    Text = "**** **** **** " + c.Number.Substring(c.Number.Length - 4),
                    Value = c.Id.ToString()
                })
                .ToList()
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Deposit(DepositViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return RedirectToAction("Deposit", "Account");
            }

            //GET CURRENT USER
            var currUser = await this.userManager.GetUserAsync(this.User);

            try
            {
                await this.accountService.DepositAsync(currUser.Id, model.Amount, TransactionType.Deposit, model.CardId);
            }
            catch (UserDoesntExistsException ex)
            {
                this.TempData["Error-Message"] = ex.Message;
                return View();
            }

            this.TempData["Success-Message"] = $"You successfully make deposit!";
            //return View("Deposit");
            return RedirectToAction("Deposit", "Account");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddCreditCard()
        {
            return View("Deposit");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddCreditCard(CreditCardViewModel model)
        {
            model.Number = model.Number.Replace(" ", string.Empty);

            if (!ModelState.IsValid)
            {
                return PartialView("_AddCardPartial");
            }

            var currUser = await this.userManager.GetUserAsync(this.User);

            try
            {
                await this.creditCardService.AddAsync(model.Number, int.Parse(model.CVV),
                    currUser.Id, model.Expiry);
            }
            catch (CreditCardAlreadyExistsException ex)
            {
                this.TempData["Error-Message"] = ex.Message;
                return View();
            }

            this.TempData["Success-Message"] = $"Card successfully added!";
            //return View("Deposit");
            return RedirectToAction("Deposit", "Account");
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateDateExpiry(DateTime expiry)
        {
            if (expiry.Year < DateTime.Now.Year)
            {
                return Json("Invalid date expiry.");
            }
            else if (expiry.Year == DateTime.Now.Year && expiry.Month < DateTime.Now.Month)
            {
                return Json("Invalid date expiry.");
            }

            return Json(true);
        }
    }
}
