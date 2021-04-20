using System;

namespace YAMEP_LEARN {
    /*
     *  Expression => "1 + 2"
     *
     *  NUMBER(1)
     *  OP(+)
     *  NUMBER(2)
     *  
     */
    class Program {
        static void Main(string[] args) {
            var expression = "1+22 -32* 4/ 5";

            var lexer = new Lexer(new SourceScanner(expression));

            while(lexer.Peek().Type != Token.TokenType.EOE) {
                var token = lexer.ReadNext();
                Console.WriteLine($"Token {token.Value} found at position {token.Position}");
            }
        }
    }
}
