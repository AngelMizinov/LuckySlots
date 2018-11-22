namespace LuckySlots.Services.Abstract
{
    using LuckySlots.Data;

    public abstract class BaseService
    {
        private readonly LuckySlotsDbContext context;

        public BaseService(LuckySlotsDbContext context)
        {
            this.context = context;
        }

        public LuckySlotsDbContext Context => this.context;
    }
}
