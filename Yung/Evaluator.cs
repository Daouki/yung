using System.Collections.Generic;
using System.Linq;
using Yung.AST;
using Yung.Exceptions;

namespace Yung
{
    public static class Evaluator
    {
        public static IValue Evaluate(IValue ast, Dictionary<Symbol, IValue> environment)
        {
            if (!(ast is List)) return EvaluateNode(ast, environment);

            if (((List) ast).Count == 0) return ast;
            var evaluated = EvaluateNode(ast, environment);
            var list = (List) evaluated;
            var function = (Function) list.Head.Value;
            var arguments = list.Tail;
            return function.Apply(arguments);
        }

        private static IValue EvaluateNode(IValue node, Dictionary<Symbol, IValue> environment)
        {
            switch (node)
            {
                case List list: return new List(EvaluateCollection(list, environment));
                case Vector vector: return new Vector(EvaluateCollection(vector, environment));
                case Symbol symbol:
                    if (!environment.TryGetValue(symbol, out var value))
                        throw new UndefinedSymbol(symbol);
                    return value;
                default:
                    return node;
            }
        }

        private static IEnumerable<IValue> EvaluateCollection(
            IEnumerable<IValue> collection,
            Dictionary<Symbol, IValue> environment)
        {
            var oldValues = collection.ToArray();
            var newValues = new IValue[oldValues.Length];
            for (var i = 0; i < oldValues.Length; i += 1)
                newValues[i] = Evaluate(oldValues[i], environment);
            return new List(newValues);
        }
    }
}
