namespace LuckySlots.App.Areas.Admin.Controllers
{
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using LuckySlots.Infrastructure;
    using LuckySlots.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [Area("Admin")]
    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    public class UsersController : Controller
    {
        private readonly IUserManagementServices userManagementServices;

        [TempData]
        public string StatusMessage { get; set; }

        public UsersController(IUserManagementServices userManagementServices)
        {
            this.userManagementServices = userManagementServices;
        }

        public IActionResult All()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReadUsers([DataSourceRequest] DataSourceRequest request)
        {
            var users = await this.userManagementServices.GetAllUsersAsync();
            return Json(users.ToDataSourceResult(request));
        }

        public IActionResult ManageUser(string id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult ManageRoles(string role)
        {
            // TODO: Create a service to update the roles of the user
            // TODO: Create a servide to lock/unlock the user
            this.StatusMessage = $"User roles updated successfully.";

            return RedirectToAction(nameof(All));
        }
    }
}
