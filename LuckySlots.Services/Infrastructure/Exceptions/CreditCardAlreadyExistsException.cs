using System;
using System.Collections.Generic;
using System.Text;

namespace LuckySlots.Services.Infrastructure.Exceptions
{
    public class CreditCardAlreadyExistsException : Exception
    {
        public CreditCardAlreadyExistsException(string message) : base(message)
        {

        }
    }
}
