namespace LuckySlots.Services.Infrastructure.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    
    public class CreditCardDoesntExistsException : Exception
    {
        public CreditCardDoesntExistsException(string message) : base(message)
        {

        }
    }
}
