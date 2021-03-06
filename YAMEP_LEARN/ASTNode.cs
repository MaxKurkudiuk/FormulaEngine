using System.Collections.Generic;

namespace YAMEP_LEARN {
    /// <summary>
    /// base class for all AST Nodes in the tree
    /// </summary>
    public abstract class ASTNode {
        public Token Token { get; private set; }
        public ASTNode(Token token) => Token = token;
    }

    public abstract class IdentifierASTNode : ASTNode {
        public string Name { get; }
        protected IdentifierASTNode(Token token) : base(token) => this.Name = token.Value;
    }

    public class VariableIdentifierASTNode : IdentifierASTNode {
        public VariableIdentifierASTNode(Token token)
            : base(token) { }
    }

    public class FunctionASTNode : IdentifierASTNode {
        public List<ASTNode> ArgumentsNodes { get; }
        public FunctionASTNode(Token token) 
            : base(token) => ArgumentsNodes = new List<ASTNode>();
    }

    /// <summary>
    /// Represents a basic number [0-9]+
    /// </summary>
    public class NumberASTNode : ASTNode {
        /// <summary>
        /// Get the Number value of the Node
        /// </summary>
        public double Value => double.Parse(Token.Value);
        public NumberASTNode(Token token) : base(token) { }
    }

    /// <summary>
    /// Base class for all operators
    /// </summary>
    public abstract class OperatorASTNode : ASTNode {
        public OperatorASTNode(Token token) : base(token) { }
    }

    public abstract class UnaryOperatorASTNode : ASTNode {
        public ASTNode Target { get; }
        protected UnaryOperatorASTNode(Token token, ASTNode target)
            : base(token) => Target = target;
    }

    public class NegationUnaryOperatorASTNode : UnaryOperatorASTNode {
        public NegationUnaryOperatorASTNode(Token token, ASTNode target) : base(token, target) {
        }
    }

    public class FactorialUnaryOperatorASTNode : UnaryOperatorASTNode {
        public FactorialUnaryOperatorASTNode(Token token, ASTNode target) : base(token, target) {
        }
    }

    /// <summary>
    /// For all binary operators => LEFT OP RIGHT
    /// </summary>
    public abstract class BinaryOperatorASTNode : OperatorASTNode {
        public ASTNode Left { get; protected set; }
        public ASTNode Right { get; protected set; }
        public BinaryOperatorASTNode(Token token, ASTNode left, ASTNode right) : base(token) {
            Left = left;
            Right = right;
        }
    }

    public class AdditionBinaryOperatorASTNode : BinaryOperatorASTNode {
        public AdditionBinaryOperatorASTNode(Token token, ASTNode left, ASTNode right) : base(token, left, right) { }
    }
    public class SubtractionBinaryOperatorASTNode : BinaryOperatorASTNode {
        public SubtractionBinaryOperatorASTNode(Token token, ASTNode left, ASTNode right) : base(token, left, right) { }
    }
    public class MultiplicationBinaryOperatorASTNode : BinaryOperatorASTNode {
        public MultiplicationBinaryOperatorASTNode(Token token, ASTNode left, ASTNode right) : base(token, left, right) { }
    }
    public class DivisionBinaryOperatorASTNode : BinaryOperatorASTNode {
        public DivisionBinaryOperatorASTNode(Token token, ASTNode left, ASTNode right) : base(token, left, right) { }
    }

    public class ExponentBinaryOperatorASTNode : BinaryOperatorASTNode {
        public ExponentBinaryOperatorASTNode(Token token, ASTNode left, ASTNode right) : base(token, left, right) {
        }
    }
}
