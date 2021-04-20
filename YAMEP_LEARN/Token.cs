namespace YAMEP_LEARN {
    public class Token {

        public enum TokenType {
            EOE,            // End of Expression - Sentinel
            NUMBER,         // [0-9]+
            Addition,       // => +
            Subtraction,    // => -
            Multiplication, // => *
            Division,       // => /
        }

        public TokenType Type { get; }
        public int Position { get; }
        public string Value { get; }
        public Token(TokenType type, int position, string value) {
            Type = type;
            Position = position;
            Value = value;
        }
    }
}
