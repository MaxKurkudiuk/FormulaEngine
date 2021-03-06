using System;
using System.Collections.Generic;
using System.Linq;

namespace YAMEP_LEARN {

    // Implicit multiplication: 2x x(1+2)
    // => 2x^2 => 2(x ^ 2)
    // 2 * -x = 2-x     FAIL!
    // -2x! <= (-1)(2*(x!))
    // 2x!x(1+2)^2
    /// <summary>
    /// Implements the following Production Rules
    ///       EXPRESSION: TERM [('+'|'-') TERM]*
    ///             TERM: FACTOR [('*'|'/') FACTOR]*
    ///           FACTOR: '-'? IMPLICIT_MUL
    ///     IMPLICIT_MUL: EXPONENT IMPLICIT_MUL*
    ///         EXPONENT: FACTORIAL_FACTOR [ '^' EXPONENT ]*
    /// FACTORIAL_FACTOR: PRIMARY '!'?
    ///          PRIMARY: IDENTIFIER | NUMBER | SUB_EXPRESSION
    ///      IDENTOIFIER: VARIABLE | FUNCTION
    ///         FUNCTION: FUNCTION_NAME '(' FUNC_ARGS ')'
    ///        FUNC_ARGS: EXPRESSION [, EXPRESSION]*
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

        readonly Lexer _lexer;
        readonly SymbolTable _symbolTable;

        public Parser(Lexer lexer) : this(lexer, new SymbolTable()) { }

        public Parser(Lexer lexer, SymbolTable symbolTable) {
            _lexer = lexer;
            _symbolTable = symbolTable;
        }

        /// <summary>
        /// Parses the suplied expression and returns the root node of the AST
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ASTNode Parse() {
            if (TryParseExpression(out ASTNode node)) {
                Expect(Token.TokenType.EOE);
                return node;
            } else
                throw new Exception($"Failed to Parse Expression Tree");
        }

        /// <summary>
        /// Parses the EXPRESSION Production Rule
        /// EXPRESSION: TERM [('+'|'-') TERM]*
        /// </summary>
        /// <returns></returns>
        private bool TryParseExpression(out ASTNode node) {
            if (TryParseTerm(out node)) {
                while (IsNext(TERM_OPERATOR)) {
                    var op = Accept();  // accept the operator
                    if (TryParseTerm(out ASTNode rhs))        // rhs = rightHendSide
                        node = CreateBinaryOperator(op, node, rhs);
                    else
                        throw new Exception($"Exception Parsing the Expression Rule at position {_lexer.Position}");
                }
            }
            return node != null;
        }

        /// <summary>
        /// Preses the TERM Production Rule                 1 * 2 * 3
        /// TERM: FACTOR [('*'|'/')] FACTOR]*                    *
        /// </summary>                                        1   *
        /// <returns></returns>                                 2   3
        private bool TryParseTerm(out ASTNode node) {
            if (TryParseFactor(out node)) {
                while (IsNext(FACTOR_OPERATOR)) {
                    var op = Accept();  // accept the operator
                    if (TryParseFactor(out ASTNode rhs))      // rhs = rightHendSide
                        node = CreateBinaryOperator(op, node, rhs);
                    else
                        throw new Exception($"Exception Parsing the Term Rule at position {_lexer.Position}");
                }
            }
            return node != null;
        }

        /// <summary>
        /// Parses the FACTOR Production Rule
        /// FACTOR: '-'? IMPLICIT_MUL
        /// </summary>
        /// <returns></returns>
        private bool TryParseFactor(out ASTNode node) {
            if (IsNext(Token.TokenType.Minus)) {
                var op = Accept();  // accept the operator
                if (TryParseImplicitMultiplication(out node))
                    node = new NegationUnaryOperatorASTNode(op, node);
                else
                    throw new Exception($"Exception Parsing the Factor Rule at position {_lexer.Position}");
            } else {
                TryParseImplicitMultiplication(out node);
            }
            return node != null;
        }

        /// <summary>
        /// Parses the IMPLICIT_MULTIPLICATION Production Rule
        /// IMPLICIT_MUL: EXPONENT IMPLICIT_MUL*
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private bool TryParseImplicitMultiplication(out ASTNode node) {
            if (TryParseExponent(out node)) {
                if (TryParseImplicitMultiplication(out ASTNode rhs))
                    node = CreateBinaryOperator(new Token(Token.TokenType.Multiplication, -1, null), node, rhs);
            }
            return node != null;
        }

        /// <summary>
        /// Parses the EXPONENT Production Rule
        /// EXPONENT: FACTORIAL_FACTOR [ '^' EXPONENT ]*
        /// Example: 2^3^2                                                   ^
        /// lift_node(2)                                                   2   ^
        /// right_node(exponent_node(left(3), right(2))                      3   2
        /// </summary>
        private bool TryParseExponent(out ASTNode node) {
            if (TryParseFactorialFactor(out node)) {
                if (IsNext(Token.TokenType.Exponent)) {
                    var op = Accept(); // accept the operator
                    if (TryParseExponent(out ASTNode rhs))    // rhs = rightHendSide
                        node = new ExponentBinaryOperatorASTNode(op, node, rhs);
                }
            }
            return node != null;
        }

        /// <summary>
        /// Preses the FACTORIAL_FACTOR Production Rule
        /// FACTORIAL_FACTOR: PRIMARY '!'?
        /// </summary>
        private bool TryParseFactorialFactor(out ASTNode node) {
            if (TryParsePrimary(out node))
                if (IsNext(Token.TokenType.Factorial))
                    node = new FactorialUnaryOperatorASTNode(Accept(), node);

            return node != null;
        }

        /// <summary>
        /// Preses the PRIMARY Production Rule
        /// PRIMARY: IDENTIFIER | NUMBER | SUB_EXPRESSION
        /// </summary>
        private bool TryParsePrimary(out ASTNode node) {
            if (!TryParseIdentifier(out node))
                if (!TryParseNumber(out node))
                    if (!TryParseSubExpression(out node))
                        return false; //throw new Exception($"Invalid Expression expected either Number or ( at {_lexer.Position}");
            return true;
        }

        /// <summary>
        /// Preses the IDENTIFIER Production Rule
        /// IDENTIFIER: VARIABLE | FUNCTION                     (_?[a-zA-Z]+[a-zA-Z0-9_]*)
        /// </summary>
        private bool TryParseIdentifier(out ASTNode node) {
            if (!TryParseVariable(out node))
                if (!TryParseFunction(out node))
                    return false;
            return true;
        }

        private bool TryParseVariable(out ASTNode node) {
            node = null;
            if (IsNext(Token.TokenType.Identifier)) {
                var token = _lexer.Peek();
                var stEntrys = _symbolTable.Get(token.Value);
                if (stEntrys == null)
                    throw new Exception($"Undefined Identifier {token.Value} at position {token.Position}");

                if (stEntrys.First().Type == SymbolTableEntry.EntryType.Variable)
                    node = new VariableIdentifierASTNode(Accept());
            }
            return node != null;
        }

        // FUNCTION: FUNCTION_NAME '(' FUNC_ARGS ')'
        private bool TryParseFunction(out ASTNode node) {
            node = null;
            if (IsNext(Token.TokenType.Identifier)) {
                var token = _lexer.Peek();
                var stEntrys = _symbolTable.Get(token.Value);
                if (stEntrys == null)
                    throw new Exception($"Undefined Identifier {token.Value} at position {token.Position}");

                if (stEntrys.All(e => e.Type == SymbolTableEntry.EntryType.Fucntion)) {
                    node = new FunctionASTNode(Accept());

                    Expect(Token.TokenType.OpenParen);
                    Accept();
                    
                    if (TryParseFuncArgs(out var arguments))
                        (node as FunctionASTNode).ArgumentsNodes.AddRange(arguments);
                    
                    Expect(Token.TokenType.CloseParen);
                    Accept();
                }
            }
            return node != null;
        }

        // FUNC_ARGS: EXPRESSION [, EXPRESSION]*
        private bool TryParseFuncArgs(out List<ASTNode> nodes) {
            nodes = new List<ASTNode>();
            if (TryParseExpression(out var node)) {
                nodes.Add(node);
                while (IsNext(Token.TokenType.ArgSeperator)) {
                    Accept();   // eat the comma
                    if (TryParseExpression(out node))
                        nodes.Add(node);
                    else
                        throw new Exception($"Exception Parsing Function Argumants at Position {_lexer.Position}");
                }
            }
            return nodes.Any();
        }

        /// <summary>
        /// Preses the NUMBER Production Rule
        /// NUMBER: [0-9]+
        /// </summary>
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
                if (TryParseExpression(out node)) {
                    Expect(Token.TokenType.CloseParen);
                    Accept();   // consume the close paren
                }
            }
            return node != null;
        }

        private void Expect(Token.TokenType tokenType) {
            if (!IsNext(tokenType))
                throw new Exception($"Unexpected token {_lexer.Peek()?.Value} at position {_lexer.Position}");
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
