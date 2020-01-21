using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Yung.AST;
using Yung.Exceptions;
using Boolean = Yung.AST.Boolean;

namespace Yung
{
    public static class Reader
    {
        /// <summary>
        ///     Transform the given source code into an abstract syntax tree.
        /// </summary>
        /// <param name="sourceCode">Source code to be transformed.</param>
        /// <returns>An abstract syntax tree that represents the given source code.</returns>
        public static IValue Read(string sourceCode)
        {
            // Tokens are grouped inside of a queue, because we only need a single token lookahead
            // in LISP, and it makes reading the tokens easier, as we don't have to pass current
            // token index everywhere.
            var tokens = Tokenize(sourceCode);
            return tokens.Count == 0 ? new Nil() : ReadForm(tokens);
        }

        /// <summary>
        ///     Transform the given source code into a queue of tokens.
        /// </summary>
        /// <param name="sourceCode">Source code to be tokenized.</param>
        /// <returns>Queue of tokens.</returns>
        private static Queue<string> Tokenize(string sourceCode)
        {
            const string pattern =
                @"[\s ,]*(~@|[\[\]{}()'`~@]|""(?:[\\].|[^\\""])*""?|;.*|[^\s \[\]{}()'""`~@,;]*)";

            var tokens = new Queue<string>();
            var regex = new Regex(pattern);
            foreach (Match match in regex.Matches(sourceCode))
            {
                var token = match.Groups[1].Value;
                if (!string.IsNullOrEmpty(token) && token[0] != ';') tokens.Enqueue(token);
            }

            return tokens;
        }

        private static IValue ReadForm(Queue<string> tokens)
        {
            var token = tokens.Dequeue();
            switch (token)
            {
                case "(": return ReadList(tokens);
                case "[": return ReadVector(tokens);
                case "{": return ReadMap();
                case "#[": return ReadSet();
                case ")":
                case "]":
                case "}": throw new YungException($"Unmatched `{token}'.");
                default: return ReadAtom(token);
            }
        }

        private static IValue ReadList(Queue<string> tokens)
        {
            var elements = new List<IValue>();
            try
            {
                while (tokens.Peek() != ")") elements.Add(ReadForm(tokens));
            }
            catch (InvalidOperationException)
            {
                throw new YungException("No matching `)' to the previously opened `('.");
            }

            tokens.Dequeue();
            return new List(elements);
        }

        private static IValue ReadVector(Queue<string> tokens)
        {
            var vector = new Vector();
            try
            {
                while (tokens.Peek() != "]") vector.Add(ReadForm(tokens));
            }
            catch (InvalidOperationException)
            {
                throw new YungException("No matching `]' to the previously opened `['.");
            }

            tokens.Dequeue();
            return vector;
        }

        private static IValue ReadMap()
        {
            throw new NotImplementedException("Maps are not available yet.");
        }

        private static IValue ReadSet()
        {
            throw new NotImplementedException("Sets are not available yet.");
        }

        private static IValue ReadAtom(string token)
        {
            switch (token)
            {
                case "nil": return new Nil();
                case "#t": return new Boolean(true);
                case "#f": return new Boolean(false);
                default:
                    const string pattern = @"(-?[0-9]*\.[0-9]*)|(-?[0-9]*)";
                    var match = Regex.Match(token, pattern);
                    if (!match.Success)
                        throw new YungException($"Token `{token}' is not a valid atom.");
                    if (!string.IsNullOrEmpty(match.Groups[1].Value))
                        return new Float(float.Parse(token,
                            CultureInfo.InvariantCulture.NumberFormat));
                    if (!string.IsNullOrEmpty(match.Groups[2].Value))
                        return new Integer(long.Parse(token));
                    throw new YungException($"Token `{token}' is not a valid atom.");
            }
        }
    }
}
