using System;

namespace YAMEP_LEARN {

    /// <summary>
    /// Implements the following Production Rules
    /// EXPRESSION: TERM [('+'|'-')] TERM*
    ///       TERM: FACTOR [('*'|'/')] FACTOR]*
    ///     FACTOR: NUMBER
    ///     NUMBER: [0-9]+
    /// </summary>
    public static class Parser {
        public static ASTNode Parse(string expression) {
            var lexer = new Lexer(new SourceScanner(expression));
            return ParseExpression(lexer);
        }
        /// EXPRESSION: TERM [('+'|'-')] TERM*
        private static ASTNode ParseExpression(Lexer lexer) {
            var left = ParseTerm(lexer);

            var lookahead = lexer.Peek();
            while(lookahead.Type == Token.TokenType.Addition || lookahead.Type == Token.TokenType.Subtraction) {
                var op = lexer.ReadNext();

                var right = ParseTerm(lexer);

                left = CreateBinaryOperator(op, left, right);

                lookahead = lexer.Peek();
            }

            return left;
        }
        /// TERM: FACTOR[('*' | '/')] FACTOR]*
        private static ASTNode ParseTerm(Lexer lexer) {

            //1 * 2 * 3
            //
            //    *
            //  1   *
            //    2   3
            var left = ParseFactor(lexer);

            var lookahead = lexer.Peek();
            while (lookahead.Type == Token.TokenType.Multiplication || lookahead.Type == Token.TokenType.Division) {
                var op = lexer.ReadNext();

                var right = ParseFactor(lexer);

                left = CreateBinaryOperator(op, left, right);

                lookahead = lexer.Peek();
            }

            return left;
        }

        /// FACTOR: NUMBER
        private static ASTNode ParseFactor(Lexer lexer) => ParseNumber(lexer);
        private static ASTNode ParseNumber(Lexer lexer) {
            Expect(lexer, Token.TokenType.Number);
            return new NumberASTNode(lexer.ReadNext());
        }

        private static ASTNode CreateBinaryOperator(Token token, ASTNode left, ASTNode right) {
            switch (token.Type) {
                case Token.TokenType.Addition: return new AdditionBinaryOperatorASTNode(token, left, right);
                case Token.TokenType.Subtraction: return new SubtractionBinaryOperatorASTNode(token, left, right);
                case Token.TokenType.Multiplication: return new MultiplicationBinaryOperatorASTNode(token, left, right);
                case Token.TokenType.Division: return new DivisionBinaryOperatorASTNode(token, left, right);
                default:
                    throw new ArgumentOutOfRangeException(nameof(token));
            }
        }

        private static void Expect(Lexer lexer, Token.TokenType expected) {
            if (lexer.Peek().Type != expected)
                throw new Exception($"Expected {expected} at Possition {lexer.Position}");

        }
    }
}
