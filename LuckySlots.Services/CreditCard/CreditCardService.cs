namespace LuckySlots.Services.CreditCard
{
    using LuckySlots.Data;
    using LuckySlots.Services.Abstract;
    using LuckySlots.Services.Contracts;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using LuckySlots.Data.Models;
    using System.Linq;
    using System;
    using Microsoft.EntityFrameworkCore;
    using LuckySlots.Services.Infrastructure.Exceptions;

    public class CreditCardService : BaseService, ICreditCardService
    {
        public CreditCardService(LuckySlotsDbContext context) : base(context)
        {

        }

        public async Task<CreditCard> GetCreditCardByIdAsync(string id)
        {
            return await this.Context.CreditCards
                  .Where(card => card.Id == new Guid(id))
                  .FirstOrDefaultAsync();
        }

        private async Task<CreditCard> GetCreditCardByNumberAsync(string number)
        {
            return await this.Context.CreditCards
              .Where(card => card.Number == number)
              .FirstOrDefaultAsync();
        }

        public async Task<string> GetLastForDigitsOfCardNumberAsync(string id)
        {
            if(id == null)
            {
                throw new CreditCardDoesntExistsException("Card number cannot be null.");
            }

            var number = await this.Context.CreditCards
                .Where(card => card.Id == new Guid(id))
                .Select(card => card.Number)
                .FirstOrDefaultAsync();
            
            return number.Substring(number.Length - 4);
        }

        public async Task<CreditCard> AddAsync(string number, int cvv, string userId, DateTime expiry)
        {
            if (number == null)
            {
                throw new CreditCardDoesntExistsException("Card number cannot be null.");
            }

            if (userId == null)
            {
                throw new UserDoesntExistsException("User Id cannot be null.");
            }

            CreditCard card = await this.GetCreditCardByNumberAsync(number);

            if (card != null)
            {
                if (card.IsDeleted == true)
                {
                    card.IsDeleted = false;

                    await this.Context.SaveChangesAsync();
                    return card;
                }

                throw new CreditCardAlreadyExistsException("Credit card with this number already exists!");
            }

            card = new CreditCard()
            {
                Number = number,
                CVV = cvv,
                UserId = userId,
                Expiry = expiry
            };

            await this.Context.CreditCards.AddAsync(card);
            await this.Context.SaveChangesAsync();

            return card;
        }

        public async Task DeleteAsync(string id)
        {
            CreditCard card = await this.GetCreditCardByIdAsync(id);

            if (card == null || card.IsDeleted == true)
            {
                throw new CreditCardDoesntExistsException("Credit card with this id does not exists!");
            }

            card.IsDeleted = true;

            await this.Context.SaveChangesAsync();
        }

        public async Task<ICollection<string>> GetAllCardsNumbersByUserId(string userId)
        {
            var numbers = await this.Context.CreditCards
                .Where(card => card.UserId == userId)
                .Select(card => card.Number)
                .ToListAsync();

            return numbers.Select(num => num.Substring(num.Length - 4)).ToList();
        }

        public async Task<ICollection<CreditCard>> GetAllByUserIdAsync(string userId)
        {
            var cards = await this.Context.CreditCards
                .Where(crCard => crCard.UserId == userId
                && crCard.IsDeleted == false)
                .ToListAsync();

            return cards;
        }

    }
}
