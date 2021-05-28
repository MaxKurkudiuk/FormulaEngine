namespace YAMEP_LEARN {
    public class ExpressionEngine {

        /// <summary>
        /// Evaluates an expression and returns the final result
        /// </summary>
        /// <param name="expression">the expression to evaluate</param>
        /// <returns></returns>
        public double Evaluate(string expression) {
            var astRoot = new Parser(new Lexer(new SourceScanner(expression))).Parse();
            return Evaluate(astRoot as dynamic);
        }

        static double Evaluate(AdditionBinaryOperatorASTNode node) => Evaluate(node.Left as dynamic) + Evaluate(node.Right as dynamic);
        static double Evaluate(SubtractionBinaryOperatorASTNode node) => Evaluate(node.Left as dynamic) - Evaluate(node.Right as dynamic);
        static double Evaluate(MultiplicationBinaryOperatorASTNode node) => Evaluate(node.Left as dynamic) * Evaluate(node.Right as dynamic);
        static double Evaluate(DivisionBinaryOperatorASTNode node) => Evaluate(node.Left as dynamic) / Evaluate(node.Right as dynamic);
        static double Evaluate(NumberASTNode node) => node.Value;
        static double Evaluate(NegationUnaryOperatorASTNode node) => -1 * Evaluate(node.Target as dynamic);
        static double Evaluate(FactorialUnaryOperatorASTNode node) {
            int fact(int x) => x == 1 ? 1 : x * fact(x - 1);
            var value = (int)Evaluate(node.Target as dynamic);
            if (value < 0)
                throw new System.Exception("Factorial only supports Positive Integers");
            value = value == 0 ? 1 : value;
            //return value < 0 ? -fact(-value) : fact(value);
            return fact(value);
        }
    }
}
