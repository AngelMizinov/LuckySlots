namespace LuckySlots.Data.Configurations
{
    using LuckySlots.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class CreditCardConfiguration : IEntityTypeConfiguration<CreditCard>
    {
        public void Configure(EntityTypeBuilder<CreditCard> builder)
        {
            builder
                .HasOne(card => card.User)
                .WithMany(card => card.CreditCards);
            
        }
    }
}
