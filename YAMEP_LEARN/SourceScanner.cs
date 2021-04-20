using System.Collections.Generic;

namespace YAMEP_LEARN {
    public class SourceScanner {

        readonly Stack<int> _positionStack = new Stack<int>();

        readonly string _buffer;    // the expression

        public int Position { get; private set; }

        public bool EndOfSource => Position >= _buffer.Length;

        public SourceScanner(string expresion) => _buffer = expresion;

        public char? Read() => EndOfSource ? (char?)null : _buffer[Position++];

        public char? Peek() {
            Push();
            var next = Read();
            Pop();
            return next;
        }

        public void Push() => _positionStack.Push(Position);

        public void Pop() => Position = _positionStack.Pop();
    }
}
