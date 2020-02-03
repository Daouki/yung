using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Yung.AST;
using Yung.Exceptions;
using String = Yung.AST.String;

namespace Yung
{
    public static class Printer
    {
        public static string Print(IValue expression)
        {
            return expression switch
            {
                Boolean boolean => boolean.Value ? "#t" : "#f",
                Integer integer => integer.Value.ToString(),
                Float floating => AddDotZeroIfInteger(floating.Value.ToString("G7",
                    CultureInfo.InvariantCulture.NumberFormat)),
                Nil _ => "nil",
                String @string => @string.Value,
                Symbol symbol => symbol.Value,
                Function _ => "#<function>",
                Keyword keyword => keyword.Value,
                List list => PrintCollection("(", ")", list),
                Vector vector => PrintCollection("[", "]", vector),
                _ => throw new YungException(
                    $"Tried to print a node of type {expression.GetType().Name}, but this action is not implemented.")
            };
        }

        private static string AddDotZeroIfInteger(string number)
        {
            return !number.Contains('.') ? number + ".0" : number;
        }

        private static string PrintCollection(
            string begin,
            string end,
            IEnumerable<IValue> collection)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(begin);
            foreach (var value in collection)
            {
                stringBuilder.Append(Print(value));
                stringBuilder.Append(' ');
            }

            if (stringBuilder.Length != 1)
                stringBuilder.Length -= 1; // Remove trailing space for non-empty collections.
            stringBuilder.Append(end);
            return stringBuilder.ToString();
        }
    }
}
