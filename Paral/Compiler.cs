#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Paral.Exceptions;
using Paral.Parsing;
using Serilog;

#endregion


namespace Paral
{
    public class Compiler
    {
        private const string _DEFAULT_TEMPLATE = "{Timestamp:MM/dd/yy-HH:mm:ss} | {Level:u3} | {Message}\r\n";

        private readonly Stopwatch _Stopwatch;
        private readonly Parser _Parser;

        private string _FilePath;

        public TimeSpan ParserTime { get; private set; }
        public TimeSpan CompileTime { get; private set; }

        public Compiler(IReadOnlyList<string> args)
        {
            _FilePath = String.Empty;

            ProcessCompilerArguments(args);
            ValidateCompilerArguments();

            _Stopwatch = new Stopwatch();
            _Parser = new Parser(File.OpenRead(_FilePath));
        }

        public static void InitializeLogger()
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console(outputTemplate: _DEFAULT_TEMPLATE).CreateLogger();
        }

        private void ProcessCompilerArguments(IReadOnlyList<string> args)
        {
            for (int index = 0; index < args.Count; index++)
            {
                switch (args[index])
                {
                    case "-in":
                        if ((index + 1) >= args.Count) ThrowHelper.Throw("Input file path not provided.");
                        else
                        {
                            index++;

                            _FilePath = args[index];
                        }

                        break;
                    default:
                        ThrowHelper.Throw($"Invalid argument \"{args[index]}\".");
                        break;
                }
            }
        }

        private void ValidateCompilerArguments()
        {
            if (string.IsNullOrEmpty(_FilePath)) ThrowHelper.Throw("Input file not provided.");
        }

        public async Task Compile()
        {
            _Stopwatch.Restart();

            await _Parser.Parse();

            ParserTime = _Stopwatch.Elapsed;

            _Stopwatch.Stop();

            CompileTime = ParserTime;

            Log.Information
            (
                $"Compiled file \"{_FilePath}\" in {CompileTime.TotalMilliseconds:0.00}ms (parser: {ParserTime.TotalMilliseconds:0.00}ms)."
            );
        }
    }
}
