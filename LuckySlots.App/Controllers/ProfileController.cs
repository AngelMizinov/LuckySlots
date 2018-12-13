namespace LuckySlots.App.Controllers
{
    using LuckySlots.App.Models;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProfileController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IUserManagementServices userManagementServices;
        private readonly ITransactionServices transactionServices;
        private readonly ICreditCardService creditCardService;

        public ProfileController(UserManager<User> userManager, IUserManagementServices userManagementServices,
            ITransactionServices transactionServices, ICreditCardService creditCardService)
        {
            this.userManager = userManager;
            this.userManagementServices = userManagementServices;
            this.transactionServices = transactionServices;
            this.creditCardService = creditCardService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var currUser = await this.userManager.GetUserAsync(this.User);

            var creditCards = await this.creditCardService.GetAllByUserIdAsync(currUser.Id);

            var transactions = await this.transactionServices.GetAllByUserIdAsync(currUser.Id);

            ProfileViewModel model = new ProfileViewModel()
            {
                UserId = currUser.Id,
                FirstName = currUser.FirstName,
                LastName = currUser.LastName,
                Email = currUser.Email,
                AccountBalance = currUser.AccountBalance,
                DateBirth = currUser.DateBirth,
                Currency = currUser.Currency,
                //CreditCards = creditCards,
                Transactions = transactions
            };

            //model.DeleteCardModel.CreditCards = currUser.CreditCards.Select(card => new SelectListItem()
            //{
            //    Text = "**** **** **** " + card.Number.Substring(card.Number.Length - 4),
            //    Value = card.Id.ToString()
            //}).ToList();

            if (this.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
            {
                return PartialView(model);
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(ProfileViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return RedirectToAction("Edit");
            }

            var currUser = await this.userManager.GetUserAsync(this.User);

            await this.userManagementServices.UpdateFirstName(currUser.Id, model.FirstName);

            await this.userManagementServices.UpdateLastName(currUser.Id, model.LastName);

            if (this.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
            {
                return PartialView();
            }

            return RedirectToAction("Edit");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DeleteCard()
        {
            var currUser = await this.userManager.GetUserAsync(this.User);

            var cards = await this.creditCardService.GetAllByUserIdAsync(currUser.Id);

            DeleteCardViewModel model = new DeleteCardViewModel()
            {
                CreditCards = cards.Select(c => new SelectListItem
                {
                    Text = "**** **** **** " + c.Number.Substring(c.Number.Length - 4),
                    Value = c.Id.ToString()
                })
                .ToList()
            };

            return View("Edit", model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteCard(CreditCardViewModel model)
        {
            if (!ModelState.IsValid)
            {
                RedirectToAction("Edit");
            }

            return View("Edit");
        }
    }
}
