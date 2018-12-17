namespace LuckySlots.App.Areas.Support.Controllers
{
    using LuckySlots.App.Areas.Support.Models;
    using LuckySlots.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    [Area("Support")]
    //[Authorize(Roles = GlobalConstants.SupportRoleName)]
    public class HomeController : Controller
    {
        private readonly IGameStatsService gameStatsService;

        public HomeController(IGameStatsService gameStatsService)
        {
            this.gameStatsService = gameStatsService;
        }

        public IActionResult Index()
        {
            var games = new string[] { "gameofcodes", "tuttifrutti", "treasuresofegypt" };
            var gameStatsList = new List<GameStats>();

            foreach (var gameName in games)
            {
                var gamesPlayed = this.gameStatsService.GetNumberOfGamesPlayedByGame(gameName);
                var amountStaked = this.gameStatsService.GetStakedAmountByGame(gameName);
                var amountPaidOut = this.gameStatsService.GetPaidOutAmountByGame(gameName);

                gameStatsList.Add(new GameStats
                {
                    Name = gameName,
                    GamesPlayed = gamesPlayed,
                    AmountStaked = amountStaked,
                    AmountPaidOut = amountPaidOut
                });
            }

            return View(gameStatsList);
        }
    }
}