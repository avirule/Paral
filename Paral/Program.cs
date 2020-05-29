#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Serilog;

#endregion

namespace Paral
{
    public enum TokenType
    {
        Identifier,
        Operator,
        CharacterLiteral,
        StringLiteral,
        NumericLiteral,
        DecimalLiteral,
        ControlFlow,
        SingularFlow,
        Expression,
        NewLine,
        EndOfFile
    }

    public class Token
    {
        public Point Location { get; }
        public TokenType TokenType { get; }
        public string Value { get; }

        public Token(Point location, TokenType tokenType, string value) => (Location, TokenType, Value) = (location, tokenType, value);
    }

    internal class Program
    {
        private const string _DEFAULT_TEMPLATE = "{Timestamp:MM/dd/yy-HH:mm:ss} | {Level:u3} | {Message}\r\n";

        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console(outputTemplate: _DEFAULT_TEMPLATE).CreateLogger();

            Scanner scanner = new Scanner(File.ReadAllText("Test.paral").ToCharArray());
            scanner.Tokenize();
        }
    }

    public class Scanner
    {
        private readonly char[] _Data;

        private int _StartIndex;
        private int _RunIndex;
        private Point _Location;
        private List<Token> _Tokens;

        public IReadOnlyList<Token> Tokens => _Tokens;

        public Scanner(char[] data)
        {
            _Data = data;
            _StartIndex = _RunIndex = 0;
            _Location = new Point(1, 1);
            _Tokens = new List<Token>();
        }

        public void Tokenize()
        {
            while (!IsEndOfFile(Scan()))
            {
                _StartIndex = _RunIndex;

                if (IsWhiteSpace(Scan()))
                {
                    SkipWhiteSpace();
                }
                else if (IsNewLine(Scan()))
                {
                    SkipNewLine();

                    _Tokens.Add(new Token(_Location, TokenType.EndOfFile, string.Empty));
                    _Location = new Point(_Location.X + 1, 1);
                }
                else if (IsStringLiteralEnclosure(Scan()))
                {
                    string str = StringLiteralClosure();
                    _Tokens.Add(new Token(_Location, TokenType.StringLiteral, str));
                }
                else if (IsDigit(Scan()))
                {
                    string number = NumericLiteralClosure(out bool isDecimal);
                    _Tokens.Add(new Token(_Location, isDecimal ? TokenType.DecimalLiteral : TokenType.NumericLiteral, number));
                }
                else if (IsAlphanumeric(Scan()))
                {
                    _Tokens.Add(new Token(_Location, TokenType.Identifier, AlphanumericClosure()));
                }
                else
                {
                    Log.Error($"[Row: {_Location.X}, Col: {_Location.Y}] Failed to read token: {Scan()}");
                    Environment.Exit(-1);
                }
            }

            // allocate EOF
            _Tokens.Add(new Token(_Location, TokenType.EndOfFile, "\0"));
        }

        private void SkipWhiteSpace()
        {
            while (IsWhiteSpace(Scan()))
            {
                Advance();
            }
        }

        private void SkipNewLine()
        {
            while (IsNewLine(Scan()))
            {
                Advance();
            }
        }

        private string StringLiteralClosure()
        {
            while (!IsStringLiteralEnclosure(Scan()))
            {
                if (IsEndOfFile(Scan()))
                {
                    Log.Error($"[Row: {_Location.X}, Col: {_Location.Y}] Token has no closure: {_Data[_StartIndex]}");
                }

                Advance();
            }

            return GatherCurrentSubset();
        }

        private string NumericLiteralClosure(out bool isDecimal)
        {
            bool decimalEncountered;
            isDecimal = false;

            while ((decimalEncountered = IsDecimal(Scan())) || IsDigit(Scan()))
            {
                if (decimalEncountered)
                {
                    // second decimal point encountered, error out
                    if (isDecimal)
                    {
                        Log.Information($"[Row: {_Location.X}, Col: {_Location.Y}] Decimal literal can only contain a single decimal point.");
                        Environment.Exit(-1);
                    }
                    else
                    {
                        isDecimal = true;
                    }
                }

                Advance();
            }

            return GatherCurrentSubset();
        }

        private string AlphanumericClosure()
        {
            while (IsAlphanumeric(Scan()) || IsDigit(Scan()))
            {
                Advance();
            }

            return GatherCurrentSubset();
        }

        private char Scan() => _RunIndex < _Data.Length ? _Data[_RunIndex] : '\0';

        private void Advance()
        {
            _RunIndex++;
            _Location.Y++;
        }

        private string GatherCurrentSubset() => new string(new ArraySegment<char>(_Data, _StartIndex, _RunIndex - _StartIndex));

        private static bool IsEndOfFile(char character) => character == '\0';

        private static bool IsWhiteSpace(char character) => (character == ' ') || (character == '\t');

        private static bool IsNewLine(char character) => (character == '\r') || (character == '\n');

        private static bool IsDigit(char character) => (character >= '0') && (character <= '9');

        private static bool IsDecimal(char character) => character == '.';

        private static bool IsAlphanumeric(char character) =>
            ((character >= 'A') && (character <= 'Z')) || ((character >= 'a') && (character <= 'z'));

        private static bool IsCharacterLiteralEnclosure(char character) => character == '\'';

        private static bool IsStringLiteralEnclosure(char character) => character == '"';
    }
}
