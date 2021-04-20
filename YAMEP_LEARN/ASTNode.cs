namespace YAMEP_LEARN {
    public abstract class ASTNode {
        public Token Token { get; private set; }
        public ASTNode(Token token) => Token = token;
    }

    public class NumberASTNode : ASTNode {
        public int Value => int.Parse(Token.Value);
        public NumberASTNode(Token token) : base(token) { }
    }

    public abstract class OperatorASTNode : ASTNode {
        public OperatorASTNode(Token token) : base(token) { }
    }

    public abstract class BinaryOperatorASTNode : OperatorASTNode {
        public ASTNode Left { get; }
        public ASTNode Right { get; }
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
}
