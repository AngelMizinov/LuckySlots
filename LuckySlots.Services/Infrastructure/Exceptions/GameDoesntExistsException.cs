namespace LuckySlots.Services.Infrastructure.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GameDoesntExistsException : Exception
    {
        public GameDoesntExistsException(string message) : base(message)
        {

        }
    }
}
