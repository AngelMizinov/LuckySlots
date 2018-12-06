namespace LuckySlots.Services.Infrastructure.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserDoesntExistsException : Exception
    {
        public UserDoesntExistsException(string message): base(message)
        {
            
        }
    }
}
