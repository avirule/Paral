#region

using System.Threading.Tasks;

#endregion

namespace Paral
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Compiler compiler = new Compiler(args);
            await compiler.Compile();
        }
    }
}
