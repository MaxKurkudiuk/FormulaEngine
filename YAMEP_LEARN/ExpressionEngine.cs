namespace YAMEP_LEARN {
    public static class ExpressionEngine {

        /// <summary>
        /// Evaluates an expression and returns the final result
        /// </summary>
        /// <param name="expression">the expression to evaluate</param>
        /// <returns></returns>
        public static double Evaluate(string expression) {
            var astRoot = Parser.Parse(expression);
            return Evaluate(astRoot as dynamic);
        }

        static double Evaluate(AdditionBinaryOperatorASTNode node) => Evaluate(node.Left as dynamic) + Evaluate(node.Right as dynamic);
        static double Evaluate(SubtractionBinaryOperatorASTNode node) => Evaluate(node.Left as dynamic) - Evaluate(node.Right as dynamic);
        static double Evaluate(MultiplicationBinaryOperatorASTNode node) => Evaluate(node.Left as dynamic) * Evaluate(node.Right as dynamic);
        static double Evaluate(DivisionBinaryOperatorASTNode node) => Evaluate(node.Left as dynamic) / Evaluate(node.Right as dynamic);
        static double Evaluate(NumberASTNode node) => node.Value;
    }
}
