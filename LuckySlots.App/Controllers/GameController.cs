namespace LuckySlots.App.Controllers
{
    using LuckySlots.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class GameController : Controller
    {
        private readonly IGameService gameService;

        public GameController(IGameService gameService)
        {
            this.gameService = gameService;
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
        public IActionResult GetGame(string gameName, decimal stake)
        {
            //var isStakeValid = this.accountService.CheckBalanceAsync(userId);
            var game = this.gameService.GetGame(gameName);
            var spinCoefficient = this.gameService.Spin(game);
            var spinResult = this.gameService.GetSpinResult(game, spinCoefficient, stake);

            //var gameOutcome = new
            //{
            //    gameGrid = new string[,]
            //    {
            //        { "haskell", "haskell", "haskell" },
            //        { "csharp", "javascript", "csharp" },
            //        { "wildcard", "csharp", "csharp" },
            //        { "haskell", "javascript", "wildcard" }
            //    },

            //    winningRows = new int[] { 1, 3 },

            //    winnings = stake * 2
            //};

            return Json(spinResult);
        }
    }
}