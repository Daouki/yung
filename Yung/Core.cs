using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using Yung.AST;
using Yung.Exceptions;
using Boolean = Yung.AST.Boolean;
using String = Yung.AST.String;

namespace Yung
{
    public static class Core
    {
        public static readonly ImmutableDictionary<Symbol, Function> Functions =
            ImmutableDictionary<Symbol, Function>.Empty
                .Add(new Symbol("+"), MakeNumberArithmeticOperation(typeof(INumber).GetMethod("Add")))
                .Add(new Symbol("-"),
                    MakeNumberArithmeticOperation(typeof(INumber).GetMethod("Subtract"),
                        value => value.Negate()))
                .Add(new Symbol("*"), MakeNumberArithmeticOperation(typeof(INumber).GetMethod("Multiply")))
                .Add(new Symbol("/"), MakeNumberArithmeticOperation(typeof(INumber).GetMethod("Divide")))
                .Add(new Symbol("<"), MakeNumberComparisonOperation(typeof(INumber).GetMethod("Less")))
                .Add(new Symbol("<="),
                    MakeNumberComparisonOperation(typeof(INumber).GetMethod("LessOrEqual")))
                .Add(new Symbol(">"), MakeNumberComparisonOperation(typeof(INumber).GetMethod("Greater")))
                .Add(new Symbol(">="),
                    MakeNumberComparisonOperation(typeof(INumber).GetMethod("GreaterOrEqual")))
                // Type predicates.
                .Add(new Symbol("is-nil?"), IsType<Nil>("nil"))
                .Add(new Symbol("is-boolean?"), IsType<Boolean>("boolean"))
                .Add(new Symbol("is-float?"), IsType<Float>("float"))
                .Add(new Symbol("is-integer?"), IsType<Integer>("integer"))
                .Add(new Symbol("is-number?"), IsType<INumber>("number"))
                .Add(new Symbol("is-keyword?"), IsType<Keyword>("keyword"))
                .Add(new Symbol("is-string?"), IsType<String>("string"))
                .Add(new Symbol("is-list?"), IsType<List>("list"))
                .Add(new Symbol("is-vector?"), IsType<Vector>("vector"))
                // String operations.
                .Add(new Symbol("to-string"), ToString())
                .Add(new Symbol("string/concatenate"), StringConcatenate())
                .Add(new Symbol("string/join"), StringJoin())
                // Console IO operations.
                .Add(new Symbol("write"), Write())
                .Add(new Symbol("write-line"), WriteLine());

        private static Function MakeNumberArithmeticOperation(
            MethodBase binaryOperation,
            Func<INumber, INumber> unaryOperation = null)
        {
            return new Function(arguments =>
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (arguments.Count == 0) return new Nil();
                if (arguments.Count == 1)
                {
                    var argument = arguments[0];
                    if (!(argument is Integer) && !(argument is Float))
                        throw new TypeMismatchException(
                            "The argument to the `+' function has to be either of type Integer or Float.");

                    return unaryOperation != null
                        ? unaryOperation((INumber) arguments[0])
                        : argument;
                }

                try
                {
                    var result = (INumber) arguments[0];
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

        private static Function MakeNumberComparisonOperation(MethodBase comparisonOperation)
        {
            return new Function(arguments =>
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (arguments.Count == 0) return new Nil();
                if (arguments.Count == 1) return new Boolean(true);

                try
                {
                    for (var i = 0; i < arguments.Count - 1; i += 1)
                        if (!((INumber) arguments[i]).Less((INumber) arguments[i + 1]).Value)
                            return Boolean.False;
                    return Boolean.True;
                }
                catch (InvalidCastException)
                {
                    throw new TypeMismatchException();
                }
            });
        }

        private static Function IsType<T>(string typeName)
        {
            return new Function(args =>
            {
                if (args.Count != 1)
                    throw new InvalidNumberOfFunctionArgumentsException(
                        $"is-{typeName}?", 1, args.Count);
                return new Boolean(args[0] is T);
            });
        }

        private new static Function ToString()
        {
            return new Function(args =>
            {
                var value = new StringBuilder();
                foreach (var arg in args)
                    value.Append(arg is String str ? str.Value : Printer.Print(arg));
                return new String(value.ToString());
            });
        }

        private static Function StringConcatenate()
        {
            return new Function(args =>
            {
                var value = new StringBuilder();
                try
                {
                    foreach (var arg in args) value.Append(((String) arg).Value);
                }
                catch (Exception)
                {
                    throw new TypeMismatchException();
                }

                return new String(value.ToString());
            });
        }

        private static Function StringJoin()
        {
            return new Function(args =>
            {
                var value = new StringBuilder();
                var separator = (String) args[0];
                for (var i = 1; i < args.Count; i += 1)
                {
                    value.Append(((String) args[i]).Value);
                    value.Append(separator.Value);
                }

                if (args.Count > 1) value.Length--;
                return new String(value.ToString());
            });
        }

        private static Function Write()
        {
            return new Function(args =>
            {
                foreach (var arg in args) Console.Write(arg is String str ? str.Value : Printer.Print(arg));
                return new Nil();
            });
        }

        private static Function WriteLine()
        {
            return new Function(args =>
            {
                foreach (var arg in args) Console.Write(arg is String str ? str.Value : Printer.Print(arg));
                Console.Write($"{System.Environment.NewLine}");
                return new Nil();
            });
        }
    }
}
