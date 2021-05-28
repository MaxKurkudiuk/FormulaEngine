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
            var expression = "5! + 4!";

            var lexer = new Lexer(new SourceScanner(expression));

            Console.WriteLine($"Lexing expression: {expression}");

            Token token = null;
            do {
                token = lexer.ReadNext();
                Console.WriteLine($"Found Token Type {token.Type} at Position {token.Position} with a value of '{token.Value}'");
            } while (token.Type != Token.TokenType.EOE);

            var evalEngine = new ExpressionEngine();
            Console.WriteLine($"{expression} = {evalEngine.Evaluate(expression)}");
            return;
            var expressions = new string[] {
                "1 + 2",        //3
                "2 * 3",        //6
                "1 + 2 * 3",    //7
                "1",
                "1 / 2"
            };

            
            foreach (var e in expressions)
                Console.WriteLine($"{e} = {evalEngine.Evaluate(e)}");
        }
    }
}
