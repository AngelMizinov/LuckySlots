namespace LuckySlots.App.Controllers
{
    using LuckySlots.App.Models;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
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

            currUser.CreditCards = await this.creditCardService.GetAllByUserIdAsync(currUser.Id);

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
                CreditCards = currUser.CreditCards,
                Transactions = transactions
            };

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
    }
}
