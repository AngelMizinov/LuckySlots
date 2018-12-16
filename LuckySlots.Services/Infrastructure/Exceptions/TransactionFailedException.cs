namespace LuckySlots.Services.Infrastructure.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TransactionFailedException : Exception
    {
        public TransactionFailedException(string message) : base(message)
        {

        }
    }
}
