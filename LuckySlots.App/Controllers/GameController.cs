namespace LuckySlots.App.Controllers
{
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure.Enums;
    using LuckySlots.Services.Contracts;
    using LuckySlots.Services.Infrastructure.Exceptions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Threading.Tasks;

    [Authorize]
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
                throw new InsufficientFundsException("You do not have enough funds for this stake!");
            }
            else if (stake < 0)
            {
                throw new ArgumentException("Stake is not valid!");
            }

            var game = this.gameService.GetGame(gameName);
            var spinCoefficient = this.gameService.Spin(game);
            var spinResult = this.gameService.GetSpinResult(game, spinCoefficient, stake);

            if (spinResult.Winnings > 0)
            {
                var newBalance = await this.accountService.DepositAsync(userId, spinResult.Winnings, TransactionType.Win, gameName: gameName);
            }
            else
            {
                var a = await this.accountService.ChargeAccountAsync(userId, stake, TransactionType.Stake, gameName: gameName);
            }

            return Json(spinResult);
        }
    }
}