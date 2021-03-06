﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Yung.AST;
using Yung.Exceptions;

namespace Yung
{
    public static class Evaluator
    {
        public static IValue Evaluate(IValue ast, Environment environment)
        {
            if (!(ast is List)) return EvaluateNode(ast, environment);

            var list = (List) ast;
            if (list.Count == 0) return ast;

            var first = list.Head.Value;
            var symbol = first is Symbol ? ((Symbol) first).Value : "__*fn*__";
            return symbol switch
            {
                "def" => EvaluateDef(list.Tail, environment),
                "do" => EvaluateDo(list.Tail, environment),
                "fn" => EvaluateFn(list.Tail, environment),
                "if" => EvaluateIf(list.Tail, environment),
                "quote" => EvaluateQuote(list.Tail),
                _ => EvaluateInvocation(list, environment)
            };
        }

        private static IValue EvaluateNode(IValue node, Environment environment)
        {
            switch (node)
            {
                case List list: return new List(EvaluateCollection(list, environment));
                case Vector vector: return new Vector(EvaluateCollection(vector, environment));
                case Symbol symbol:
                    var value = environment.GetValue(symbol);
                    if (value == null) throw new UndefinedSymbolException(symbol);
                    return value;
                default:
                    return node;
            }
        }

        /// <summary>
        ///     Evaluates all elements of the collection in order.
        /// </summary>
        /// <param name="collection">Collection to be evaluated.</param>
        /// <param name="environment">The current environment.</param>
        /// <returns>Collection with the same length as input, with all of its elements evaluated.</returns>
        private static IEnumerable<IValue> EvaluateCollection(
            IEnumerable<IValue> collection,
            Environment environment)
        {
            var oldValues = collection.ToArray();
            var newValues = new IValue[oldValues.Length];
            for (var i = 0; i < oldValues.Length; i += 1)
                newValues[i] = Evaluate(oldValues[i], environment);
            return new List(newValues);
        }

        private static IValue EvaluateDef(
            IEnumerable<IValue> arguments,
            Environment environment)
        {
            // (def name init)
            // (def name [args] body)
            var argumentArray = arguments as IValue[] ?? arguments.ToArray();
            return argumentArray.Length switch
            {
                2 => EvaluateBindingDefinition(argumentArray, environment),
                3 => EvaluateFunctionDefinition(argumentArray, environment),
                _ => throw new InvalidNumberOfFunctionArgumentsException(
                    "def", new[] {2, 3}, argumentArray.Length)
            };
        }

        private static IValue EvaluateBindingDefinition(
            IReadOnlyList<IValue> arguments,
            Environment environment)
        {
            var name = arguments[0] as Symbol ?? throw new TypeMismatchException(
                           $"Expected the first argument of `def' to be a Symbol, but got {arguments[0].GetType()} instead.");
            var init = Evaluate(arguments[1], environment);
            environment.Add(name, init);
            return name;
        }

        private static IValue EvaluateFunctionDefinition(
            IReadOnlyList<IValue> arguments,
            Environment environment)
        {
            var name = arguments[0] as Symbol ?? throw new TypeMismatchException(
                           "Expected the first argument of `def' to be a Symbol, " +
                           $"but got {arguments[0].GetType().Name} instead.");
            var parameters = arguments[1] as Vector ?? throw new TypeMismatchException(
                                 "Expected the second argument of `def' to be a Vector, " +
                                 $"but got {arguments[0].GetType().Name} instead.");
            var body = arguments[2];

            IValue Function(Vector functionArguments) =>
                Evaluate(body, new Environment(environment, parameters, functionArguments));

            var function = new Function(Function, parameters, body, environment);
            environment.Add(name, function);
            return name;
        }

        private static IValue EvaluateDo(IEnumerable<IValue> expressions, Environment environment)
        {
            // (do expr*)
            var expressionArray = expressions.ToArray();

            // Evaluate all the arguments and yield the last one.
            if (expressionArray.Length == 0) return new Nil();
            for (var i = 0; i < expressionArray.Length - 1; i += 1)
                Evaluate(expressionArray[i], environment);
            return Evaluate(expressionArray[^1], environment);
        }

        private static IValue EvaluateFn(IEnumerable<IValue> arguments, Environment environment)
        {
            // (fn [args] body)
            var argumentArray = arguments as IValue[] ?? arguments.ToArray();
            AssertFunctionArgumentCount("if", 2, argumentArray.Length);

            var parameters = argumentArray[0] as Vector ??
                             throw new TypeMismatchException(
                                 "Expected the 1st argument to `fn' to be a Vector, " +
                                 $"but got {argumentArray[0].GetType().Name} instead.");
            var body = argumentArray[1];

            IValue Function(Vector arguments) =>
                Evaluate(body, new Environment(environment, parameters, arguments));

            return new Function(Function, parameters, body, environment);
        }

        private static IValue EvaluateIf(IEnumerable<IValue> argument, Environment environment)
        {
            // (if test then else)
            var argumentArray = argument.ToArray();
            AssertFunctionArgumentCount("if", 3, argumentArray.Length);

            var condition = Evaluate(argumentArray[0], environment);

            // If the 'test' condition is either a Nil or #f or an empty list, evaluate and yield
            // the 'else' expression. In any other case, evaluate and yield the 'then' expression.
            return Evaluate(
                IsFalsehood(condition) ? argumentArray[2] : argumentArray[1],
                environment);
        }

        private static bool IsFalsehood(IValue form)
        {
            return form is Nil ||
                   form is Boolean boolean && !boolean.Value ||
                   form is List list && list.Count == 0;
        }

        private static IValue EvaluateQuote(IEnumerable<IValue> arguments)
        {
            var argumentArray = arguments as IValue[] ?? arguments.ToArray();
            AssertFunctionArgumentCount("quote", 1, argumentArray.Length);
            return argumentArray[0];
        }

        private static IValue EvaluateInvocation(IValue invocation, Environment environment)
        {
            // Head of the invocation is the function to be called, tail are the arguments to that
            // function.
            var evaluated = (List) EvaluateNode(invocation, environment);
            var function = (Function) evaluated.Head.Value;
            var arguments = evaluated.Tail.ToVector();
            return function.Apply(arguments);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AssertFunctionArgumentCount(string function, int expected, int given)
        {
            if (expected != given)
                throw new InvalidNumberOfFunctionArgumentsException(function, expected, given);
        }
    }
}
