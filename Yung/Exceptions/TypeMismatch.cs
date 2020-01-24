using System;

namespace Yung.Exceptions
{
    public class TypeMismatch : YungException
    {
        public TypeMismatch()
        {
        }

        public TypeMismatch(string? message) : base(message)
        {
        }

        public TypeMismatch(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
