namespace YAMEP_LEARN {
    public static class ExpressionEngine {
        public static int Evaluate(string expression) {
            var astRoot = Parser.Parse(expression);

            return Evaluate(astRoot as dynamic);
        }

        static int Evaluate(NumberASTNode node) => node.Value;

        static int Evaluate(AdditionBinaryOperatorASTNode node)
            => Evaluate(node.Left as dynamic) + Evaluate(node.Right as dynamic);
        static int Evaluate(SubtractionBinaryOperatorASTNode node)
            => Evaluate(node.Left as dynamic) - Evaluate(node.Right as dynamic);
        static int Evaluate(MultiplicationBinaryOperatorASTNode node)
            => Evaluate(node.Left as dynamic) * Evaluate(node.Right as dynamic);
        static int Evaluate(DivisionBinaryOperatorASTNode node)
            => Evaluate(node.Left as dynamic) / Evaluate(node.Right as dynamic);
    }
}
