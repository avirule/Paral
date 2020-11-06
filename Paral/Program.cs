#region

using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Paral.Parsing;
using Paral.Parsing.Nodes;
using Serilog;

#endregion


namespace Paral
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            Compiler.InitializeLogger();
            Parser parser = new Parser(File.OpenRead("TestFiles/Test_Functions.paral"));
            Module module = await parser.Parse();

            Log.Information($"Total run time: {stopwatch.Elapsed.TotalMilliseconds:0.00}ms");

            if (module.Completed) return;
        }
    }
}
