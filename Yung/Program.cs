using System;
using System.IO;
using System.Reflection;
using CommandLine;
using Yung.AST;
using Yung.Exceptions;
using Yung.Options;

namespace Yung
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0) REPL(MakeInitialEnvironment());
            else
                Parser.Default.ParseArguments<InteractiveOptions, RunOptions>(args)
                    .WithParsed<InteractiveOptions>(opts => RunREPL(false))
                    .WithParsed<RunOptions>(opts => RunFile(opts.File, opts.RunInteractivePrompt));
        }

        private static void RunREPL(bool printVersion)
        {
            if (printVersion)
            {
                var version = Assembly.GetEntryAssembly()?.GetName().Version;
                Console.WriteLine($"yung v{version} {System.Environment.NewLine}");
            }

            REPL(MakeInitialEnvironment());
        }

        private static void RunFile(string filePath, bool runREPL)
        {
            var sourceCode = File.ReadAllText(filePath);
            var abstractSyntaxTree = Read(sourceCode);
            var environment = MakeInitialEnvironment();
            Evaluate(abstractSyntaxTree, environment);
            if (runREPL) REPL(environment);
        }

        private static Environment MakeInitialEnvironment()
        {
            var environment = new Environment();
            foreach (var (key, value) in Core.Functions) environment.Add(key, value);
            Evaluate(Read("(def not [x] (if x #f #t))"), environment);
            Evaluate(Read("(def when [test body] (if test body nil))"), environment);
            Evaluate(Read("(def unless [test body] (if test nil body))"), environment);
            return environment;
        }

        /// <summary>
        ///     Read-evaluate-print loop. Get the input from the user, parse it, evaluate and print
        ///     the result back to the screen. Repeat until the world dies. Or when the user wants
        ///     to stop. Whatever.
        /// </summary>
        private static void REPL(Environment environment)
        {
            while (true)
                try
                {
                    Console.Write("yung> ");
                    var input = Console.ReadLine();
                    var output = Print(Evaluate(Read(input), environment));
                    Console.WriteLine(output);
                }
                catch (FeatureNotImplementedException e)
                {
                    Console.Error.WriteLine($"Error: Feature not implemented: {e.Message}");
                }
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

        private static IValue Evaluate(IValue abstractSyntaxTree, Environment environment)
        {
            return Evaluator.Evaluate(abstractSyntaxTree, environment);
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
