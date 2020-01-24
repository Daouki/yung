using System;
using Yung.AST;

namespace Yung.Exceptions
{
    public class SymbolRedefinitionException : YungException
    {
        public SymbolRedefinitionException()
        {
        }

        public SymbolRedefinitionException(string message) : base(message)
        {
        }

        public SymbolRedefinitionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SymbolRedefinitionException(Symbol symbol)
            : base($"Symbol with the name `{symbol.Value}' already exists in the current scope.")
        {
        }
    }
}
