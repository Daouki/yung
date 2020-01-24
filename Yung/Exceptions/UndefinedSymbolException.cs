using System;
using Yung.AST;

namespace Yung.Exceptions
{
    public class UndefinedSymbolException : YungException
    {
        public UndefinedSymbolException()
        {
        }

        public UndefinedSymbolException(string? message) : base(message)
        {
        }

        public UndefinedSymbolException(string? message, Exception? innerException) : base(message,
            innerException)
        {
        }

        public UndefinedSymbolException(Symbol symbol)
            : base($"Symbol `{symbol}' was not defined in the current scope.")
        {
        }
    }
}
