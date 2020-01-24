using System;
using Yung.AST;

namespace Yung.Exceptions
{
    public class UndefinedSymbol : YungException
    {
        public UndefinedSymbol()
        {
        }

        public UndefinedSymbol(string? message) : base(message)
        {
        }

        public UndefinedSymbol(string? message, Exception? innerException) : base(message,
            innerException)
        {
        }

        public UndefinedSymbol(Symbol symbol)
            : base($"Symbol `{symbol}' was not defined in the current scope.")
        {
        }
    }
}
