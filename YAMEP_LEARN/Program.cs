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
            Console.WriteLine(Math.Abs(4));
            Console.WriteLine(Math.Pow(4, 4));
            Console.WriteLine(Math.Exp(4));
            Console.WriteLine(System.Double.MaxValue);
            //Console.WriteLine(Math.Log10(10)); return;

            //SymbolTable symbol = new SymbolTable();
            //symbol.AddFunction<SupportedFunctions>();
            //foreach (var function in symbol.GetAll()) {
            //    Console.WriteLine(function.Key);
            //}

            //var entry = symbol.Get("Sin") as FunctionSymbolTableEntry;
            //var result = entry.MethodInfo.Invoke(null, new object[] { 5 });
            //Console.WriteLine(result);

            //var funcAST = new FunctionASTNode(new Token(Token.TokenType.Identifier, 0, "Sin"));
            //var arg = new NumberASTNode(new Token(Token.TokenType.Number, 0, "5"));
            //funcAST.ArgumentsNodes.Add(arg);
            //var ee = new ExpressionEngine();
            //ee.AddFunctions<SupportedFunctions>();
            //var result = ee.Evaluate(funcAST);
            //Console.WriteLine(result);

            var expEngine = new ExpressionEngine();
            expEngine.AddFunctions<SupportedFunctions>();
            var result = expEngine.Evaluate("Log(5)");
            Console.WriteLine(result);
            result = expEngine.Evaluate("Log(5, 5)");
            Console.WriteLine(result);
            return;

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
