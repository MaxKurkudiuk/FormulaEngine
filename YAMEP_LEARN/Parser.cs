using System;
using System.Linq;

namespace YAMEP_LEARN {

    /// <summary>
    /// Implements the following Production Rules
    ///       EXPRESSION: TERM [('+'|'-')] TERM*
    ///             TERM: FACTOR [('*'|'/')] FACTOR]*
    ///           FACTOR: '-'? EXPONENT
    ///         EXPONENT: FACTORIAL_FACTOR [ '^' EXPONENT ]*
    /// FACTORIAL_FACTOR: PRIMARY '!'?
    ///          PRIMARY: NUMBER | SUB_EXPRESSION
    ///   SUB_EXPRESSION: '(' EXPRESSION ')'
    /// 
    /// // Parser Rules
    /// expression
    ///         : term
    ///         | expression '+' term
    ///         | expression '-' term
    /// 
    /// // Lexer Rules
    /// INTEGER: [0-9]+
    /// FP_NUM: [0-9]* .? INTEGER
    /// </summary>
    public class Parser {

        static readonly Token.TokenType[] TERM_OPERATOR     = new Token.TokenType[] {Token.TokenType.Addition, Token.TokenType.Minus};
        static readonly Token.TokenType[] FACTOR_OPERATOR   = new Token.TokenType[] {Token.TokenType.Multiplication, Token.TokenType.Division};

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

            while (IsNext(TERM_OPERATOR)) {
                var op = Accept();  // accept the operator
                var right = ParseTerm();
                left = CreateBinaryOperator(op, left, right);
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

            while (IsNext(FACTOR_OPERATOR)) {
                var op = Accept();  // accept the operator
                var right = ParseFactor();
                left = CreateBinaryOperator(op, left, right);
            }

            return left;
        }

        /// <summary>
        /// Parses the FACTOR Production Rule
        /// FACTOR: '-'? EXPONENT
        /// </summary>
        /// <returns></returns>
        private ASTNode ParseFactor() {
            ASTNode node = default;

            if (IsNext(Token.TokenType.Minus)) {
                node = new NegationUnaryOperatorASTNode(Accept(), ParseExponent());
            } else {
                node = ParseExponent();
            }

            return node;
        }

        /// <summary>
        /// Parses the EXPONENT Production Rule
        /// EXPONENT: FACTORIAL_FACTOR [ '^' EXPONENT ]*
        /// </summary>
        private ASTNode ParseExponent() {
            ASTNode node = ParseFactorialFactor();

            if (IsNext(Token.TokenType.Exponent)) {
                var op = Accept(); // accept the operator
                // example 2^3^2
                // lift_node(2)
                // right_node(exponent_node(left(3), right(2))
                //      ^
                //    2   ^
                //      3   2
                node = new ExponentBinaryOperatorASTNode(op, node, ParseExponent());
            }

            return node;
        }

        /// <summary>
        /// Preses the FACTORIAL_FACTOR Production Rule
        /// FACTORIAL_FACTOR: PRIMARY '!'?
        /// </summary>
        private ASTNode ParseFactorialFactor() {
            ASTNode node = ParsePrimary();

            if (IsNext(Token.TokenType.Factorial)) {
                node = new FactorialUnaryOperatorASTNode(Accept(), node);
            }

            return node;
        }

        /// <summary>
        /// Preses the PRIMARY Production Rule
        /// PRIMARY: NUMBER | SUB_EXPRESSION
        /// </summary>
        private ASTNode ParsePrimary() {
            if (TryParseNumber(out var node)) 
                return node;

            if (TryParseSubExpression(out node)) 
                return node;

            // we go boom!
            throw new Exception($"Invalid Expression expected either Number or ( at {_lexer.Position}");
        }

        /// <summary>
        /// Preses the NUMBER Production Rule
        /// NUMBER: [0-9]+
        /// </summary>
        /// <param name="lexer"></param>
        /// <returns></returns>
        private bool TryParseNumber(out ASTNode node) {
            node = null;
            if (IsNext(Token.TokenType.Number)) {
                node = new NumberASTNode(Accept());
            }
            return node != null;
        }

        /// <summary>
        /// SUB_EXPRESSION: '(' EXPRESSION ')'
        /// </summary>
        private bool TryParseSubExpression(out ASTNode node) {
            node = null;
            if (IsNext(Token.TokenType.OpenParen)) {
                Accept();   // consume the open parren
                node = ParseExpression();
                Expect(Token.TokenType.CloseParen);
                Accept();   // consume the close paren
            }

            return node != null;
        }

        private void Expect(Token.TokenType tokenType) {
            if (!IsNext(tokenType))
                throw new Exception($"Unexpected token {_lexer.Peek()} at position {_lexer.Position}");
        }

        private bool IsNext(params Token.TokenType[] possibleTokens)
            => IsNext(x => possibleTokens.Any(k => k == x));

        private bool IsNext(Predicate<Token.TokenType> match)
            => match(_lexer.Peek().Type);

        private ASTNode CreateBinaryOperator(Token token, ASTNode left, ASTNode right) {
            switch (token.Type) {
                case Token.TokenType.Addition: return new AdditionBinaryOperatorASTNode(token, left, right);
                case Token.TokenType.Minus: return new SubtractionBinaryOperatorASTNode(token, left, right);
                case Token.TokenType.Multiplication: return new MultiplicationBinaryOperatorASTNode(token, left, right);
                case Token.TokenType.Division: return new DivisionBinaryOperatorASTNode(token, left, right);
                default:
                    throw new ArgumentOutOfRangeException(nameof(token));
            }
        }

        private Token Accept() => _lexer.ReadNext();
    }
}
