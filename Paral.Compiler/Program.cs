#region

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Paral.Compiler.Parsing;
using Paral.Compiler.Parsing.Nodes;
using Serilog;

#endregion


namespace Paral.Compiler
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {

            Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            Stopwatch stopwatch = Stopwatch.StartNew();

            Parser parser = new Parser(File.OpenRead("TestFiles/Test_Functions.paral"));
            Module module = await parser.Parse();

            Log.Information($"Total run time: {stopwatch.Elapsed.TotalMilliseconds:0.00}ms");

            if (module.Completed) return;
        }
    }
}
