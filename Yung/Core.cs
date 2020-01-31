using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Yung.AST;
using Yung.Exceptions;
using Boolean = Yung.AST.Boolean;

namespace Yung
{
    public static class Core
    {
        public static readonly ImmutableDictionary<Symbol, Function> Functions =
            ImmutableDictionary<Symbol, Function>.Empty
                .Add(new Symbol("+"), MakeNumberOperation(typeof(INumber).GetMethod("Add")))
                .Add(new Symbol("-"),
                    MakeNumberOperation(typeof(INumber).GetMethod("Subtract"),
                        value => value.Negate()))
                .Add(new Symbol("*"), MakeNumberOperation(typeof(INumber).GetMethod("Multiply")))
                .Add(new Symbol("/"), MakeNumberOperation(typeof(INumber).GetMethod("Divide")))
                .Add(new Symbol("is-nil?"),
                    new Function(args => new Boolean(args[0] is Nil)))
                .Add(new Symbol("is-boolean?"),
                    new Function(args => new Boolean(args[0] is Boolean)))
                .Add(new Symbol("is-float?"),
                    new Function(args => new Boolean(args[0] is Float)))
                .Add(new Symbol("is-integer?"),
                    new Function(args => new Boolean(args[0] is Integer)))
                .Add(new Symbol("is-number?"),
                    new Function(args => new Boolean(args[0] is INumber)))
                .Add(new Symbol("is-keyword?"),
                    new Function(args => new Boolean(args[0] is Keyword)))
                .Add(new Symbol("is-list?"),
                    new Function(args => new Boolean(args[0] is List)))
                .Add(new Symbol("is-vector?"),
                    new Function(args => new Boolean(args[0] is Vector)))
                .Add(new Symbol("write"),
                    new Function(args =>
                    {
                        foreach (var arg in args) Console.Write(arg);
                        return new Nil();
                    }))
                .Add(new Symbol("writeln"),
                    new Function(args =>
                    {
                        foreach (var arg in args) Console.Write(Printer.Print(arg));
                        Console.WriteLine();
                        return new Nil();
                    }));

        private static Function MakeNumberOperation(
            MethodBase binaryOperation,
            Func<INumber, INumber> unaryOperation = null)
        {
            return new Function(arguments =>
            {
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
    }
}
