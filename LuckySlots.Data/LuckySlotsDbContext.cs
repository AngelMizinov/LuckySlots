namespace LuckySlots.Data
{
    using LuckySlots.Data.Configurations;
    using LuckySlots.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class LuckySlotsDbContext : IdentityDbContext<User>
    {
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        
        public LuckySlotsDbContext(DbContextOptions<LuckySlotsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new CreditCardConfiguration());
            //builder.ApplyConfiguration(new TransactionConfiguration());
            //builder.ApplyConfiguration(new UserConfiguration());
            
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
