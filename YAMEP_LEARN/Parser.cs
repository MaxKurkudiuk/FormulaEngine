using System;

namespace YAMEP_LEARN {

    /// <summary>
    /// Implements the following Production Rules
    /// EXPRESSION: TERM [('+'|'-')] TERM*
    ///       TERM: FACTOR [('*'|'/')] FACTOR]*
    ///     FACTOR: NUMBER
    ///     NUMBER: [0-9]+
    /// </summary>
    public class Parser {

        Lexer _lexer;

        public Parser(Lexer lexer) => _lexer = lexer;

        /// <summary>
        /// Parses the suplied expression and returns the root node of the AST
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ASTNode Parse() => ParseExpression();

        /// <summary>
        /// Parses the EXPRESSION Production Rule
        /// EXPRESSION: TERM [('+'|'-')] TERM*
        /// </summary>
        /// <returns></returns>
        private ASTNode ParseExpression() {
            var left = ParseTerm();

            var peekToken = _lexer.Peek();       // look a head 1 token
            while(peekToken.Type == Token.TokenType.Addition || peekToken.Type == Token.TokenType.Subtraction) {
                var op = _lexer.ReadNext();      // read the operator

                var right = ParseTerm();

                if (right == null)
                    throw new Exception($"Invalid Expression. TERM Expected at position {_lexer.Position}");

                left = CreateBinaryOperator(op, left, right);

                peekToken = _lexer.Peek();       // look a head of 1 token LL(k) where K = 1
            }

            return left;
        }

        /// <summary>
        /// Preses the TERM Production Rule
        /// TERM: FACTOR[('*'|'/')] FACTOR]*
        /// </summary>
        /// <returns></returns>
        private ASTNode ParseTerm() {

            //1 * 2 * 3
            //
            //    *
            //  1   *
            //    2   3
            var left = ParseFactor();

            var peekToken = _lexer.Peek();       // look a head 1 token
            while (peekToken.Type == Token.TokenType.Multiplication || peekToken.Type == Token.TokenType.Division) {
                var op = _lexer.ReadNext();      // read the operator

                var right = ParseFactor();

                if (right == null)
                    throw new Exception($"Invalid Expression. FOCTOR Expected at position {_lexer.Position}");

                left = CreateBinaryOperator(op, left, right);

                peekToken = _lexer.Peek();       // look a head of token LL(k) whwre K = 1
            }

            return left;
        }

        /// <summary>
        /// Parses the FACTOR Production Rule
        /// FACTOR: NUMBER
        /// </summary>
        /// <returns></returns>
        private ASTNode ParseFactor() => ParseNumber();

        /// <summary>
        /// Preses the NUMBER Production Rule
        /// NUMBER: [0-9]+
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        private ASTNode ParseNumber() {
            var token = _lexer.Peek();
            if (token.Type != Token.TokenType.Number)
                throw new Exception($"Invalid Expression. NUMBER Expected at position {_lexer.Position}");

            Accept();

            return new NumberASTNode(token);
        }

        private ASTNode CreateBinaryOperator(Token token, ASTNode left, ASTNode right) {
            switch (token.Type) {
                case Token.TokenType.Addition: return new AdditionBinaryOperatorASTNode(token, left, right);
                case Token.TokenType.Subtraction: return new SubtractionBinaryOperatorASTNode(token, left, right);
                case Token.TokenType.Multiplication: return new MultiplicationBinaryOperatorASTNode(token, left, right);
                case Token.TokenType.Division: return new DivisionBinaryOperatorASTNode(token, left, right);
                default:
                    throw new ArgumentOutOfRangeException(nameof(token));
            }
        }

        private void Accept() => _lexer.ReadNext();
    }
}
