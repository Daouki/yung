using System;
using System.Collections;
using System.Collections.Generic;
using Yung.AST;

namespace Yung.Exceptions
{
    public class InvalidNumberOfFunctionArgumentsException : YungException
    {
        public InvalidNumberOfFunctionArgumentsException()
        {
        }

        public InvalidNumberOfFunctionArgumentsException(string message) : base(message)
        {
        }

        public InvalidNumberOfFunctionArgumentsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public InvalidNumberOfFunctionArgumentsException(in string name, in int expected, in int given)
            : base($"Call to the function `{name}' expected {expected} arguments, " + 
                   $"but {given} were given.")
        {
        }
        
        public InvalidNumberOfFunctionArgumentsException(in string name, in IEnumerable<int> expected, in int given)
            : base($"Call to the function `{name}' expected " +
                   $"either {string.Join(" or  ", expected)} arguments, " + 
                   $"but {given} were given.")
        {
        }
    }
}
