using System;

namespace Yung.Exceptions
{
    public class YungException : Exception
    {
        public YungException()
        {
        }

        public YungException(string? message) : base(message)
        {
        }

        public YungException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
