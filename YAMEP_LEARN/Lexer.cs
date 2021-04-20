using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YAMEP_LEARN {
    // Implements the following matching rules
    // OPERATOR: + | - | * | /
    //   NUMBER: [0-9]+
    public class Lexer {

        const char PLUS                 = '+';
        const char MINUS                = '-';
        const char MULTIPLICATION       = '*';
        const char DIVISION             = '/';
        const char DECIMAL_SEPERATOR    = '.';

        static readonly char[] E_NOTAION            = new char[] { 'e', 'E' };
        static readonly char[] SIGN_OPEERATORS      = new char[] { PLUS, MINUS };

        readonly static Dictionary<char, Func<int, char, Token>> OperatorMap = new Dictionary<char, Func<int, char, Token>> {
            { PLUS, (p, v) => new Token(Token.TokenType.Addition, p, v.ToString())},
            { MINUS, (p, v) => new Token(Token.TokenType.Subtraction, p, v.ToString())},
            { MULTIPLICATION, (p, v) => new Token(Token.TokenType.Multiplication, p, v.ToString())},
            { DIVISION, (p, v) => new Token(Token.TokenType.Division, p, v.ToString())},
        };

        readonly SourceScanner _scanner;

        /// <summary>
        /// Current Position within the Source Scanner
        /// </summary>
        public int Position => _scanner.Position;

        /// <summary>
        /// Creates an instance of the lexer with the specified <see cref="=SourceScanner">
        /// </summary>
        /// <param name="scanner">The <see cref="SourceScanner"/> to read from</param>
        public Lexer(SourceScanner scanner) => _scanner = scanner;

        /// <summary>
        /// Read the next token
        /// </summary>
        /// <returns></returns>
        public Token ReadNext() {
            if (_scanner.EndOfSource)
                return new Token(Token.TokenType.EOE, _scanner.Position, null);

            ConsumeWhiteSpace();

            // Attempt to parse out the supported tokens

            if (TryTokenizeOperator(out var token))
                return token;

            if (TryTokenizeNumber(out token))
                return token;

            // Not good to be here
            throw new Exception($"Unexpected character {_scanner.Peek()} found at Position {_scanner.Position}");
        }

        /// <summary>
        /// Peek at the next token
        /// </summary>
        /// <returns></returns>
        public Token Peek() {
            _scanner.Push();
            var token = ReadNext();
            _scanner.Pop();
            return token;
        }

        /// <summary>
        /// Try to lex an operator
        /// </summary>
        /// <param name="token">Token representing the Operator if found</param>
        /// <returns></returns>
        private bool TryTokenizeOperator(out Token token) {
            token = null;

            var lookahead = _scanner.Peek();
            if (lookahead.HasValue && OperatorMap.ContainsKey(lookahead.Value))
                token = OperatorMap[lookahead.Value](_scanner.Position, _scanner.Read().Value) ;

            return token != null;
        }

        /// Try to lex a valid digit [0-9]+
        /*private bool TryTokenizeNumber(out Token token) {
            token = null;

            bool isDigit(char? c) => c.HasValue && char.IsDigit(c.Value);
            if (isDigit(_scanner.Peek())) {
                var position = _scanner.Position;
                var sb = new StringBuilder();
                while (isDigit(_scanner.Peek()))
                    sb.Append(_scanner.Read().Value);

                token = new Token(Token.TokenType.Number, position, sb.ToString());
            }

            return token != null;
        }*/

        /// <summary>
        /// Examples 1 100 1.5 .5 1e5 .1e5 1e-5
        /// \d* .? \d+ ( [eE] [-+]? \d+ )?
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private bool TryTokenizeNumber(out Token token) {
            token = null;
            var sb = new StringBuilder();
            var position = Position;        // where did we start finding the digit

            // \d*
            sb.Append(ReadDigits());

            // .?
            if (IsNext(DECIMAL_SEPERATOR)) {
                sb.Append(Accept());    // accept the decimal
            }

            // \d+
            sb.Append(ReadDigits());

            // ( [eE] [-+]? \d+ )?
            if (sb.Length > 0 && char.IsDigit(sb[sb.Length - 1]) && IsNext(E_NOTAION)) {
                sb.Append(Accept());                // accept the e

                if (IsNext(SIGN_OPEERATORS)) {      // accept + | -
                    sb.Append(Accept());
                }
                Expect(char.IsDigit);
                sb.Append(ReadDigits());
            }

            if (sb.Length > 0)   // found something
                token = new Token(Token.TokenType.Number, position, sb.ToString());

            if (token != null && !double.TryParse(token.Value, out _))
                throw new Exception($"Invalid numeric value {token.Value} found at position {token.Position}");

            return token != null;
        }

        private string ReadDigits() {
            var sb = new StringBuilder();
            while (IsNext(char.IsDigit))
                sb.Append(Accept());
            return sb.ToString();
        }

        private char Accept() => _scanner.Read().Value;

        private void Expect(Predicate<char> match) {
            if (!IsNext(match))
                throw new Exception($"Unexpected value at position {Position}");
        }

        private bool IsNext(params char[] possibleValues)
            => IsNext(x => possibleValues.Any(k => k == x));

        private bool IsNext(Predicate<char> match) {
            var lookahead = _scanner.Peek();
            return lookahead.HasValue && match(lookahead.Value);
        }

        /// <summary>
        /// Gobble, gobble, gobble
        /// </summary>
        private void ConsumeWhiteSpace() {
            bool isWhiteSpace(char? c) => c.HasValue && char.IsWhiteSpace(c.Value);
            while (isWhiteSpace(_scanner.Peek()))
                _scanner.Read();
        }
    }
}
