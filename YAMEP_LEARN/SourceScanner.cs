using System.Collections.Generic;

namespace YAMEP_LEARN {

    /// <summary>
    /// Provides single character scanning of source (expression)
    /// Provides Peek a head Functionality
    /// Provides Push/Pop of current position
    /// </summary>
    public class SourceScanner {

        // the expression we are scanning
        readonly string _buffer;

        // position stack for Save/Restore of position for read a head
        readonly Stack<int> _positionStack = new Stack<int>();

        /// <summary>
        /// Current Location within the Buffer
        /// Zero (0) based Position
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        /// Indicates if we are at the end of the Source Buffer
        /// </summary>
        public bool EndOfSource => Position >= _buffer.Length;

        /// <summary>
        /// Creates an instance of the SourceScanner using the specified source
        /// </summary>
        /// <param name="expresion">The source string to scan</param>
        public SourceScanner(string expresion) => _buffer = expresion;

        /// <summary>
        /// Read the next character from the source buffer
        /// </summary>
        /// <returns></returns>
        public char? Read() => EndOfSource ? (char?)null : _buffer[Position++];

        /// <summary>
        /// Look a head 1 character in the source buffer
        /// </summary>
        /// <returns></returns>
        public char? Peek() {
            Push();
            var next = Read();
            Pop();
            return next;
        }

        /// <summary>
        /// Save the Current Position
        /// </summary>
        public void Push() => _positionStack.Push(Position);

        /// <summary>
        /// Restore last Position
        /// </summary>
        public void Pop() => Position = _positionStack.Pop();
    }
}
