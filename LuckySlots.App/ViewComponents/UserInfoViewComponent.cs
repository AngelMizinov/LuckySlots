namespace LuckySlots.App.ViewComponents
{
    using LuckySlots.App.Models;
    using LuckySlots.Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class UserInfoViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(User user)
        {
            UserInfoComponentViewModel model = new UserInfoComponentViewModel()
            {
                FirstName = user.FirstName,
                AccoutBalance = user.AccountBalance,
                Currency = user.Currency
            };

            return View(model);
        }
    }
}
