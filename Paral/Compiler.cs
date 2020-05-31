#region

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Paral.Parsing;
using Serilog;

#endregion

namespace Paral
{
    public class Compiler
    {
        private readonly Stopwatch _Stopwatch;
        private readonly string _FilePath;
        private readonly Parser _Parser;

        public TimeSpan ParserTime { get; private set; }
        public TimeSpan CompileTime { get; private set; }

        public Compiler(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Log.Error($"File not found at path: {filePath}");
                Environment.Exit(-1);
            }

            _Stopwatch = new Stopwatch();
            _FilePath = filePath;
            _Parser = new Parser(File.OpenRead(_FilePath));
        }

        public async Task Compile()
        {
            _Stopwatch.Restart();

            await _Parser.Parse();

            ParserTime = _Stopwatch.Elapsed;

            _Stopwatch.Stop();

            CompileTime = ParserTime;
        }
    }
}
