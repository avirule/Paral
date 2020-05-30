#region

using Serilog;

#endregion

namespace Paral
{
    internal class Program
    {
        private const string _DEFAULT_TEMPLATE = "{Timestamp:MM/dd/yy-HH:mm:ss} | {Level:u3} | {Message}\r\n";
        private const string _FILE_NAME = "Test.paral";

        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console(outputTemplate: _DEFAULT_TEMPLATE).CreateLogger();

            Compiler compiler = new Compiler(_FILE_NAME);
            compiler.Compile();

            Log.Information
            (
                $"Compiled file \"{_FILE_NAME}\" in (parser: {compiler.ParserTime.TotalMilliseconds:0.00}ms, overall: {compiler.CompileTime.TotalMilliseconds:0.00}ms)."
            );
        }
    }
}
