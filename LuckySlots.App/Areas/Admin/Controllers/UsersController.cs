namespace LuckySlots.App.Areas.Admin.Controllers
{
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using LuckySlots.Infrastructure;
    using LuckySlots.Services.Contracts;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using System.Linq;
    using LuckySlots.App.Areas.Admin.Models;

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

        public async Task<IActionResult> All()
        {
            var users = await this.userManagementServices.GetAllUsersAsync();
            return View(users.ToList());
        }

        [HttpPost]
        public async Task<IActionResult> ReadUsers([DataSourceRequest] DataSourceRequest request)
        {
            var users = await this.userManagementServices.GetAllUsersAsync();
            return Json(users.ToDataSourceResult(request));
        }

        public async Task<IActionResult> Details([FromQuery]string userId)
        {
            var user = await this.userManagementServices.GetUserByIdAsync(userId);

            var vm = new ManageUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsAdmin = user.IsAdmin,
                IsSupport = user.IsSupport,
                IsAccountLocked = user.IsAccountLocked
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Details(ManageUserViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return View();
            }

            var user = await this.userManagementServices.GetUserByIdAsync(model.Id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public IActionResult ManageRoles(string userId, string role)
        {
            // TODO: Create a service to update the roles of the user
            // TODO: Create a servide to lock/unlock the user
            this.userManagementServices.ToggleRole(this.User.Identity.Name, role);
            this.StatusMessage = $"User roles updated successfully.";

            return RedirectToAction(nameof(All));
        }
    }
}
