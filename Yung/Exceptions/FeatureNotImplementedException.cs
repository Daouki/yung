using System;

namespace Yung.Exceptions
{
    public class FeatureNotImplementedException : YungException
    {
        public FeatureNotImplementedException()
        {
        }

        public FeatureNotImplementedException(string? message) : base(message)
        {
        }

        public FeatureNotImplementedException(string? message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
