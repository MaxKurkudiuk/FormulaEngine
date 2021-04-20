using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using YAMEP_LEARN;

namespace YAMEP_LEARNTest {
    [TestClass]
    public class UnitTest1 {

        [TestMethod()]
        public void ReadNextTest() {
            var exprission = "1 + 2";

            (Token.TokenType, int, string)[] expectedResults = new (Token.TokenType, int, string)[] {
                (Token.TokenType.NUMBER, 0, "1"),
                (Token.TokenType.Addition, 2, "+"),
                (Token.TokenType.NUMBER, 4, "2"),
            };

            var lexer = new Lexer(new SourceScanner(exprission));

            foreach (var (t, p, v) in expectedResults) {
                var token = lexer.ReadNext();
                Assert.AreEqual(t, token.Type);
                Assert.AreEqual(p, token.Position);
                Assert.AreEqual(v, token.Value);
            }

            Assert.AreEqual(Token.TokenType.EOE, lexer.ReadNext().Type);
        }
    }
}
