using System;
using System.Linq;
using System.Reflection;
using Yung.AST;
using Yung.Exceptions;

namespace Yung
{
    public static class Core
    {
        public static readonly Function Add =
            MakeNumberOperation(typeof(INumber).GetMethod("Add"));

        public static readonly Function Subtract =
            MakeNumberOperation(typeof(INumber).GetMethod("Subtract"), arg => arg.Negate());

        public static readonly Function Multiply =
            MakeNumberOperation(typeof(INumber).GetMethod("Multiply"));

        public static readonly Function Divide =
            MakeNumberOperation(typeof(INumber).GetMethod("Divide"));

        private static Function MakeNumberOperation(
            MethodBase binaryOperation,
            Func<INumber, INumber> unaryOperation = null)
        {
            return new Function(arguments =>
            {
                if (arguments.Count == 0) return new Nil();
                if (arguments.Count == 1)
                {
                    var argument = arguments.Head.Value;
                    if (!(argument is Integer) && !(argument is Float))
                        throw new TypeMismatchException(
                            "The argument to the `+' function has to be either of type Integer or Float.");

                    return unaryOperation != null
                        ? unaryOperation((INumber) arguments.Head.Value)
                        : argument;
                }

                try
                {
                    var result = (INumber) arguments.Head.Value;
                    var expressionType = result.GetType();
                    var rest = arguments.ToArray()[1..];
                    foreach (var value in rest)
                    {
                        if (value.GetType() != expressionType) throw new TypeMismatchException();
                        result = (INumber) binaryOperation.Invoke(result, new object[] {value});
                    }

                    return result;
                }
                catch (InvalidCastException)
                {
                    throw new TypeMismatchException();
                }
            });
        }
    }
}
