using System;
using System.Reflection;

namespace Yung
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine($"yung v{Assembly.GetEntryAssembly()?.GetName().Version} {Environment.NewLine}");
        }
    }
}
