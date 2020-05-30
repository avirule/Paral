#region

using System;
using System.Diagnostics;
using System.IO;
using Paral.Lexing;
using Paral.Parsing;
using Serilog;

#endregion

namespace Paral
{
    public class Compiler
    {
        private readonly Stopwatch _Stopwatch;
        private readonly string _FilePath;
        private readonly Lexer _Lexer;
        private readonly Parser _Parser;

        public TimeSpan LexerTime { get; private set; }
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

            string data = File.ReadAllText(_FilePath);
            _Lexer = new Lexer(data.ToCharArray());
            _Parser = new Parser(_Lexer);
        }

        public void Compile()
        {
            _Stopwatch.Restart();

            _Lexer.Tokenize();

            LexerTime = _Stopwatch.Elapsed;

            _Stopwatch.Restart();

            _Parser.Parse();

            ParserTime = _Stopwatch.Elapsed;

            _Stopwatch.Stop();

            CompileTime = LexerTime + ParserTime;
        }
    }
}
