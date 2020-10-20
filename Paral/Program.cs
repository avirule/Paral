#region

using System.IO;
using System.Threading.Tasks;
using Paral.Parsing;
using Paral.Parsing.Nodes;

#endregion


namespace Paral
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Compiler.InitializeLogger();

            Parser parser = new Parser(File.OpenRead("Test.paral"));
            MasterNode masterNode = await parser.Parse();
        }
    }
}
