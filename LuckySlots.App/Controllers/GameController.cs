namespace LuckySlots.App.Controllers
{
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Enums;
    using LuckySlots.Services.Contracts;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class GameController : Controller
    {
        private readonly IGameService gameService;
        private readonly IAccountService accountService;
        private readonly UserManager<User> userManager;

        public GameController(IGameService gameService, IAccountService accountService, UserManager<User> userManager)
        {
            this.gameService = gameService;
            this.accountService = accountService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GameOfCodes()
        {
            return View();
        }

        public IActionResult TuttiFrutti()
        {
            return View();
        }

        public IActionResult TreasuresOfEgypt()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetGame(string gameName, decimal stake)
        {
            var userId = this.userManager.GetUserId(this.User);

            var balance = await this.accountService.CheckBalanceAsync(userId);

            if (stake > balance)
            {
                // TODO: Catch exception in ajax error
                throw new InsufficientFundsException("You do not have enought funds for this stake!");
            }

            var game = this.gameService.GetGame(gameName);
            var spinCoefficient = this.gameService.Spin(game);
            var spinResult = this.gameService.GetSpinResult(game, spinCoefficient, stake);

            if (spinResult.Winnings > 0)
            {
                var newBalance = await this.accountService.DepositAsync(userId, spinResult.Winnings, TransactionType.Win);
            }
            else
            {
                var a = await this.accountService.ChargeAccountAsync(userId, stake, TransactionType.Stake);
            }

            return Json(spinResult);
        }
    }
}