using System;

namespace Yung.Exceptions
{
    public class TypeMismatchException : YungException
    {
        public TypeMismatchException()
        {
        }

        public TypeMismatchException(string? message) : base(message)
        {
        }

        public TypeMismatchException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
