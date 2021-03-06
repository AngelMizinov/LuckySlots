﻿namespace LuckySlots.App
{
    using LuckySlots.App.Hubs;
    using LuckySlots.Data;
    using LuckySlots.Data.Extensions;
    using LuckySlots.Data.Models;
    using LuckySlots.Infrastructure;
    using LuckySlots.Infrastructure.Contracts;
    using LuckySlots.Infrastructure.Games;
    using LuckySlots.Infrastructure.HttpClient;
    using LuckySlots.Infrastructure.Providers;
    using LuckySlots.Services.Account;
    using LuckySlots.Services.Admin;
    using LuckySlots.Services.Contracts;
    using LuckySlots.Services.CreditCard;
    using LuckySlots.Services.Games;
    using LuckySlots.Services.Transactions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Serialization;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<LuckySlotsDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<User, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<LuckySlotsDbContext>()
                .AddDefaultTokenProviders();

            services.AddHttpClient<IExchangeRateHttpClient, ExchangeRateHttpClient>();
            services.AddSingleton<IJsonParser, JsonParser>();
            services.AddSingleton<IEmailSender, EmailSender>();

            // Register services
            services.AddTransient<IUserManagementServices, UserManagementServices>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICreditCardService, CreditCardService>();
            services.AddScoped<ITransactionServices, TransactionServices>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ISpinResult, SpinResult>();
            services.AddSingleton<IGameFactory, GameFactory>();
            services.AddTransient<IRandomizer, Randomizer>();
            services.AddScoped<IGameStatsService, GameStatsService>();

            services.AddMvc(options =>
            {
                options.Filters.Add<AutoValidateAntiforgeryTokenAttribute>();

            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddRazorPagesOptions(options =>
            {
                options.AllowAreas = true;
                options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
            })
            .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddKendo();
            services.AddSignalR();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDatabaseMigration();
            app.UseRoleSeeder();
            app.UseAccountSeeder();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chat");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
