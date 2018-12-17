namespace LuckySlots.Services.Tests.UserManagementServicesTests
{
    using LuckySlots.Data;
    using LuckySlots.Data.Models;
    using LuckySlots.Services.Admin;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    [TestClass]
    public class ToggleRole_Should
    {
        private DbContextOptions<LuckySlotsDbContext> GetDbContextOptions(string dbName)
            => new DbContextOptionsBuilder<LuckySlotsDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;


        private static IRoleStore<IdentityRole> identityRoleStoreMoq = new Mock<IRoleStore<IdentityRole>>().Object;
        private static RoleManager<IdentityRole> identityRoleManagerMoq = new RoleManager<IdentityRole>(identityRoleStoreMoq, null, null, null, null);

        private static Mock<IRoleStore<User>> userRoleStoreMoq = new Mock<IRoleStore<User>>();
        private static Mock<RoleManager<User>> userRoleManagerMoq = new Mock<RoleManager<User>>(userRoleStoreMoq.Object, null, null, null, null);

        private static Mock<IUserStore<User>> userStoreMoq = new Mock<IUserStore<User>>();
        private static UserManager<User> userManagerMoq = new UserManager<User>(userStoreMoq.Object, null, null, null, null, null, null, null, null);

        //[TestMethod]
        //public async Task ThrowsException_When_UserIsNull()
        //{
        //    // Arrange
        //    var options = GetDbContextOptions("ThrowsException_When_UserIsNull");

        //    using (var context = new LuckySlotsDbContext(options))
        //    {
        //        var sut = new UserManagementServices(context, userManagerMoq, identityRoleManagerMoq);

        //        User user = null;
        //        string role = "Admin";

        //        await Assert.ThrowsExceptionAsync<NullReferenceException>(() =>

        //        sut.ToggleRole("testName", role));
        //    }
        //}

        [TestMethod]
        public async Task ThrowsException_When_RoleDoesntExists()
        {
            // Arrange
            var options = GetDbContextOptions("ThrowsException_When_RoleDoesntExists");

            using (var context = new LuckySlotsDbContext(options))
            {
                var sut = new UserManagementServices(context, userManagerMoq, identityRoleManagerMoq);
                var userId = Guid.NewGuid().ToString();

                var user = new User()
                {
                    UserName = "angel",
                    Id = userId
                };

                string role = "Admin";

                await Assert.ThrowsExceptionAsync<NullReferenceException>(() =>
                    sut.ToggleRole(user.UserName, role));
            }
        }

        //[TestMethod]
        //public async Task ThrowsException_When_UserIsAlreadyInThisRole()
        //{
        //    // Arrange
        //    var options = GetDbContextOptions("ThrowsException_When_UserIsAlreadyInThisRole");

        //    using (var context = new LuckySlotsDbContext(options))
        //    {
        //        var sut = new UserManagementServices(context, userManagerMoq, identityRoleManagerMoq);
        //        var userId = Guid.NewGuid().ToString();

        //        var user = new User()
        //        {
        //            UserName = "angel",
        //            Id = userId
        //        };

        //        var userRole = "Admin";

        //        var role = new IdentityRole();
        //        role.Name = "Admin";

        //        var userRole2 = new IdentityUserRole<string>()
        //        {
        //            RoleId = role.Id,
        //            UserId = user.Id
        //        };

        //        await this.userManagerMoq.CreateAsync(user);


        //        await context.Users.AddAsync(user);
        //        await context.Roles.AddAsync(role);
        //        await context.UserRoles.AddAsync(userRole2);
        //        await context.SaveChangesAsync();

        //        await userRoleManagerMoq.Object.CreateAsync(user);

        //        await Assert.ThrowsExceptionAsync<ArgumentException>(() =>
        //            sut.ToggleRole(user.UserName, userRole));
        //    }
        //}

        //[TestMethod]
        //public async Task AddUser_ToRole()
        //{
        //    // Arrange
        //    var options = GetDbContextOptions("AddUser_ToRole");

        //    using (var context = new LuckySlotsDbContext(options))
        //    {
        //        //var role = new IdentityRole
        //        //{
        //        //    Name = "Administrator"
        //        //};

        //        var roleStore2 = new RoleStore<IdentityRole<string>>(context);
        //        var roleManagerMoq = new Mock<RoleManager<IdentityRole>>();
        //        roleManagerMoq.Setup(x => x.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

        //        var userId = Guid.NewGuid().ToString();

        //        var user = new User()
        //        {
        //            Id = userId,
        //            IsAdmin = false
        //        };

        //        //var role = new IdentityRole();
        //        //role.Name = "Admin";

        //        //var userRole2 = new IdentityUserRole<string>()
        //        //{
        //        //    RoleId = role.Id,
        //        //    UserId = user.Id
        //        //};

        //        await context.Users.AddAsync(user);
        //        //await context.Roles.AddAsync(role);
        //        await context.SaveChangesAsync();

        //        var sut = new UserManagementServices(context, userManagerMoq, roleManagerMoq.Object);

        //        var result = await sut.ToggleRole(user, "Administrator");

        //        //var roleExists = await context.UserRoles.AnyAsync(userRole => userRole.UserId == user.Id && userRole.RoleId == role.Id);

        //        Assert.IsTrue(user.IsAdmin);
        //    }
        //}

    }
}
