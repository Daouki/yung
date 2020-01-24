using System;
using System.Linq;
using System.Reflection;
using Yung.AST;
using Yung.Exceptions;
using Boolean = Yung.AST.Boolean;

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

        public static Function IsNil = new Function(args => new Boolean(args.Head.Value is Nil));

        public static readonly Function IsBoolean =
            new Function(args => new Boolean(args.Head.Value is Boolean));

        public static Function
            IsFloat = new Function(args => new Boolean(args.Head.Value is Float));

        public static readonly Function IsInteger =
            new Function(args => new Boolean(args.Head.Value is Integer));

        public static readonly Function IsNumber =
            new Function(args => new Boolean(args.Head.Value is INumber));

        public static readonly Function IsKeyword =
            new Function(args => new Boolean(args.Head.Value is Keyword));

        public static readonly Function IsList = new Function(args => new Boolean(args.Head.Value is List));

        public static readonly Function IsVector =
            new Function(args => new Boolean(args.Head.Value is Vector));

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
