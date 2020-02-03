using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using Yung.AST;
using Yung.Exceptions;
using Boolean = Yung.AST.Boolean;
using String = Yung.AST.String;

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

            var form = ReadForm(tokens);
            if (tokens.Count == 0) return form;

            // This is kind of a hack; if there is more than a single statement, group them into
            // a Vector - this will evaluate all the nodes, but won't try to call the first
            // element as a function.
            var ast = new List<IValue> {form};
            while (tokens.Count != 0) ast.Add(ReadForm(tokens));
            return new Vector(ast);
        }

        /// <summary>
        ///     Transform the given source code into a queue of tokens.
        /// </summary>
        /// <param name="sourceCode">Source code to be tokenized.</param>
        /// <returns>Queue of tokens.</returns>
        private static Queue<string> Tokenize(string sourceCode)
        {
            const string pattern =
                @"[\s,]*([\[\]{}()']|""(?:[\\].|[^\\""])*""?|;.*|[^\s \[\]{}()'"",;]*)";

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
                case "'": return ReadQuote(tokens);
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
            throw new FeatureNotImplementedException("Maps are not available yet.");
        }

        private static IValue ReadSet()
        {
            throw new FeatureNotImplementedException("Sets are not available yet.");
        }

        private static IValue ReadQuote(Queue<string> tokens)
        {
            var form = ReadForm(tokens);
            return new List {form, new Symbol("quote")};
        }
        
        private static IValue ReadAtom(string token)
        {
            Debug.Assert(token != null);
            
            if (token[0] == '"')
            {
                if (token[^1] != '"') throw new YungException("Unterminated string.");
                return new String(token);
            }
            
            switch (token)
            {
                case "nil": return new Nil();
                case "#t": return new Boolean(true);
                case "#f": return new Boolean(false);
                default:
                    const string pattern =
                        @"^(?:(-?[0-9]+\.[0-9]+)|(-?[0-9]+)|" +
                        @"(:?(?:[a-z]|[A-Z]|[!$%^&*_\-+=<>?/])(?:[a-z]|[A-Z]|[0-9]|[!$%^&*_\-+=<>?/])*))$";
                    var match = Regex.Match(token, pattern);
                    if (!match.Success)
                        throw new YungException($"Token `{token}' is not a valid atom.");
                    if (!string.IsNullOrEmpty(match.Groups[1].Value))
                        return new Float(float.Parse(token,
                            CultureInfo.InvariantCulture.NumberFormat));
                    if (!string.IsNullOrEmpty(match.Groups[2].Value))
                        return new Integer(long.Parse(token));
                    if (!string.IsNullOrEmpty(match.Groups[3].Value))
                        return token[0] == ':' ? (IValue) new Keyword(token) : new Symbol(token);
                    throw new YungException($"Token `{token}' is not a valid atom.");
            }
        }
    }
}
