namespace LuckySlots.App.Controllers
{
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using LuckySlots.Data;
    using LuckySlots.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    

    public class TransactionsController : Controller
    {
        private readonly ITransactionServices transactionServices;
        private readonly LuckySlotsDbContext dbContext;

        public TransactionsController(ITransactionServices transactionServices, LuckySlotsDbContext dbContext)
        {
            this.transactionServices = transactionServices;
            this.dbContext = dbContext;
        }

        public IActionResult Transactions()
        {
            return PartialView();
        }

        // CRUD Actions below
        [HttpPost]
        public async Task<IActionResult> ReadTransactions([DataSourceRequest] DataSourceRequest request)
        {
            var transactions = await this.transactionServices.GetAllAsync();

            return Json(transactions.ToDataSourceResult(request));
        }
    }
}