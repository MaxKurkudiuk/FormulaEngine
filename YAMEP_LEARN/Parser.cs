﻿using System;
using System.Linq;

namespace YAMEP_LEARN {

    /// <summary>
    /// Implements the following Production Rules
    /// EXPRESSION: TERM [('+'|'-')] TERM*
    ///       TERM: FACTOR [('*'|'/')] FACTOR]*
    ///     FACTOR: NUMBER
    ///     NUMBER: [0-9]+
    /// </summary>
    public class Parser {

        static readonly Token.TokenType[] TERM_OPERATOR     = new Token.TokenType[] {Token.TokenType.Addition, Token.TokenType.Subtraction};
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
            Expect(Token.TokenType.Number);
            return new NumberASTNode(Accept());
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
                case Token.TokenType.Subtraction: return new SubtractionBinaryOperatorASTNode(token, left, right);
                case Token.TokenType.Multiplication: return new MultiplicationBinaryOperatorASTNode(token, left, right);
                case Token.TokenType.Division: return new DivisionBinaryOperatorASTNode(token, left, right);
                default:
                    throw new ArgumentOutOfRangeException(nameof(token));
            }
        }

        private Token Accept() => _lexer.ReadNext();
    }
}
