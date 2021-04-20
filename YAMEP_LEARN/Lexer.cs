using System;
using System.Collections.Generic;
using System.Text;

namespace YAMEP_LEARN {
    // Implements the following matching rules
    // OPERATOR: + | - | * | /
    //   NUMBER: [0-9]+
    public class Lexer {

        readonly static Dictionary<char, Func<int, char, Token>> OperatorMap = new Dictionary<char, Func<int, char, Token>> {
            { '+', (p, v) => new Token(Token.TokenType.Addition, p, v.ToString())},
            { '-', (p, v) => new Token(Token.TokenType.Subtraction, p, v.ToString())},
            { '*', (p, v) => new Token(Token.TokenType.Multiplication, p, v.ToString())},
            { '/', (p, v) => new Token(Token.TokenType.Division, p, v.ToString())},
        };

        readonly SourceScanner _scanner;

        public int Position => _scanner.Position;

        public Lexer(SourceScanner scanner) => _scanner = scanner;

        public Token ReadNext() {
            if (_scanner.EndOfSource)
                return new Token(Token.TokenType.EOE, _scanner.Position, null);

            ConsumeWhiteSpace();

            if (TryTokenizeOperator(out var token))
                return token;

            if (TryTokenizeNumber(out token))
                return token;

            // if we get here, go boom
            throw new Exception($"Unexpected character {_scanner.Peek()} found at Position {_scanner.Position}");
        }
        public Token Peek() {
            _scanner.Push();
            var token = ReadNext();
            _scanner.Pop();
            return token;
        }

        private bool TryTokenizeOperator(out Token token) {
            token = null;

            var lookahead = _scanner.Peek();
            if (lookahead.HasValue && OperatorMap.ContainsKey(lookahead.Value))
                token = OperatorMap[lookahead.Value](_scanner.Position, _scanner.Read().Value) ;

            return token != null;
        }
        private bool TryTokenizeNumber(out Token token) {
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
        }

        private void ConsumeWhiteSpace() {
            bool isWhiteSpace(char? c) => c.HasValue && char.IsWhiteSpace(c.Value);
            while (isWhiteSpace(_scanner.Peek()))
                _scanner.Read();
        }
    }
}
