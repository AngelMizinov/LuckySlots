namespace LuckySlots.Data.Extensions
{
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app
                .ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                serviceScope
                    .ServiceProvider
                    .GetService<LuckySlotsDbContext>()
                    .Database
                    .Migrate();
            }

            return app;
        }

        public static IApplicationBuilder UseRoleSeeder(this IApplicationBuilder app)
        {
            using (var serviceScope = app
                .ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var roleManager = serviceScope
                    .ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

                SeedRole(roleManager, GlobalConstants.AdministratorRoleName);
                SeedRole(roleManager, GlobalConstants.SupportRoleName);
            }

            return app;
        }

        public static IApplicationBuilder UseAccountSeeder(this IApplicationBuilder app)
        {
            using (var serviceScope = app
                .ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var userManager = serviceScope
                    .ServiceProvider
                    .GetRequiredService<UserManager<User>>();

                var roleManager = serviceScope
                    .ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

                SeedAccount(
                    userManager,
                    roleManager,
                    GlobalConstants.AdministratorRoleName,
                    GlobalConstants.AdministratorEmail,
                    GlobalConstants.AdministratorPassword);
            }

            return app;
        }

        private static void SeedAccount(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            string roleName,
            string email,
            string password)
        {
            var user = new User
            {
                Email = email,
                UserName = email
            };

            Task.Run(async () =>
            {
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, roleName);
            })
            .Wait();
        }

        private static void SeedRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            Task.Run(async () =>
            {
                var roleExists = await roleManager.RoleExistsAsync(roleName);

                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole
                    {
                        Name = roleName
                    });
                }
            })
            .Wait();
        }
    }
}
