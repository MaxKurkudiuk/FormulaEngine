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

            //SymbolTable symbol = new SymbolTable();
            //symbol.AddFunction<MyFunctions>();
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
            //ee.AddFunctions<MyFunctions>();
            //var result = ee.Evaluate(funcAST);
            //Console.WriteLine(result);

            var expEngine = new ExpressionEngine();
            expEngine.AddFunctions<MyFunctions>();
            var result = expEngine.Evaluate("Sin(5)");
            Console.WriteLine(result);
            result = expEngine.Evaluate("Sin2(5, 5)");
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

    public class MyFunctions {
        public static double Sin(double a) => Math.Sin(a);

        public static double Sin2(double a, double b) => Math.Sin(a + b);
        public static double Cos(double a) => Math.Cos(a);
        public static double Tg(double a) => Math.Tan(a);
        public static double Ctg(double a) => 1.0 / Math.Tan(a);

        public static string Not_Me_1(double a) => $"{a}";
        public static double Not_Me_2(string a) => a.Length;
        public static double Not_Me_3(string a, double b) => a.Length + b;
    }
}
