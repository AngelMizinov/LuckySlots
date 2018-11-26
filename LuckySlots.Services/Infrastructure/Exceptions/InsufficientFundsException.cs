namespace LuckySlots.Services.Infrastructure.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(string message) : base(message)
        {

        }
    }
}
