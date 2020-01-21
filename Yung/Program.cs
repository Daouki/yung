using System;
using System.Reflection;
using Yung.AST;
using Yung.Exceptions;

namespace Yung
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var version = Assembly.GetEntryAssembly()?.GetName().Version;
            Console.WriteLine($"yung v{version} {Environment.NewLine}");

            REPL();
        }

        /// <summary>
        ///     Read-evaluate-print loop. Get the input from the user, parse it, evaluate and print
        ///     the result back to the screen. Repeat until the world dies. Or when the user wants
        ///     to stop. Whatever.
        /// </summary>
        private static void REPL()
        {
            while (true)
                try
                {
                    Console.Write("yung> ");
                    var input = Console.ReadLine();
                    var output = Print(Evaluate(Read(input)));
                    Console.WriteLine(output);
                }
                // catch (NotImplementedException e)
                // {
                //     Console.Error.WriteLine($"Error: Feature not implemented: {e.Message}");
                // }
                catch (YungException e)
                {
                    Console.Error.WriteLine($"Error: {e.Message}");
                }
        }

        /// <summary>
        ///     Parse the given source code into abstract syntax tree.
        /// </summary>
        /// <param name="sourceCode">The source code to be transformed into abstract syntax tree.</param>
        /// <returns>Abstract syntax tree that represents the given source code.</returns>
        private static IValue Read(string sourceCode)
        {
            return Reader.Read(sourceCode);
        }

        private static IValue Evaluate(IValue abstractSyntaxTree)
        {
            return abstractSyntaxTree;
        }

        /// <summary>
        ///     Stringify the given expression.
        /// </summary>
        /// <param name="expression">Expression to be printed.</param>
        /// <returns>The given expression in a string format.</returns>
        private static string Print(IValue expression)
        {
            return Printer.Print(expression);
        }
    }
}
