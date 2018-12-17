namespace LuckySlots.App.Controllers
{
    using LuckySlots.App.Models;
    using LuckySlots.App.Models.ProfileViewModels;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Contracts;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System.Linq;
    using System.Threading.Tasks;

    [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> Info()
        {
            var currUser = await this.userManager.GetUserAsync(this.User);
            //
             //await this.userManagementServices.ToggleRole(currUser, "Administrator");
            //

            var cards = await this.creditCardService.GetAllByUserIdAsync(currUser.Id);

            DeleteCardViewModel deleteCardModel = new DeleteCardViewModel();

            deleteCardModel.CreditCards = cards.Select(card => new SelectListItem()
            {
                Text = "**** **** **** " + card.Number.Substring(card.Number.Length - 4),
                Value = card.Id.ToString()
            }).ToList();

            BalanceViewModel balanceModel = new BalanceViewModel()
            {
                FirstName = currUser.FirstName,
                LastName = currUser.LastName,
                AccountBalance = currUser.AccountBalance,
                DeleteCardModel = deleteCardModel
            };

            ProfileDetailsViewModel profileDetailsModel = new ProfileDetailsViewModel()
            {
                UserId = currUser.Id,
                FirstName = currUser.FirstName,
                LastName = currUser.LastName,
                DateBirth = currUser.DateBirth,
                Email = currUser.Email,
                Currency = currUser.Currency
            };

            InfoViewModel model = new InfoViewModel()
            {
                UserId = currUser.Id,
                BalanceModel = balanceModel,
                ProfileDetailsModel = profileDetailsModel
            };

            if (this.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
            {
                return PartialView(model);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Info(ProfileDetailsViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return RedirectToAction("Info");
            }

            var currUser = await this.userManager.GetUserAsync(this.User);

            await this.userManagementServices.UpdateFirstName(currUser.Id, model.FirstName);

            await this.userManagementServices.UpdateLastName(currUser.Id, model.LastName);

            if (this.HttpContext.Request.Headers["x-requested-with"] == "XMLHttpRequest")
            {
                return PartialView();
            }

            return RedirectToAction("Info");
        }

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

            return RedirectToAction("Info", model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCard(DeleteCardViewModel model)
        {
            if (!ModelState.IsValid)
            {
               return RedirectToAction("Info");
            }

            try
            {
                await this.creditCardService.DeleteAsync(model.CardId);
            }
            catch (CreditCardDoesntExistsException)
            {
                return RedirectToAction("Info");
            }

            this.TempData["Success-Message"] = "The card is removed successfully.";
            return RedirectToAction("Info");
        }
        
    }
}
