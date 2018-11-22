namespace LuckySlots.App.Areas.Admin.Controllers
{
    using LuckySlots.App.Areas.Admin.Models;
    using LuckySlots.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;

    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUserManagementServices userManagementServices;

        public UsersController(IUserManagementServices userManagementServices)
        {
            this.userManagementServices = userManagementServices;
        }

        public IActionResult All()
        {
            var users = this.userManagementServices.GetAllUsers();

            var vms = users
                .Select(u => new UserListViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email
                })
                .ToList();

            return View(vms);
        }
    }
}
