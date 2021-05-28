using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace YAMEP_LEARN.Tests {
    [TestClass]
    public class ExponentTests {
        [TestMethod]
        public void Test_001() {
            var expression = "2^3";
            var expressionEngine = new ExpressionEngine();

            Assert.AreEqual(8, expressionEngine.Evaluate(expression));
        }
        [TestMethod]
        public void Test_002() {
            var expression = "2^(3^2)"; // => 2^9 => 512
            var expressionEngine = new ExpressionEngine();

            Assert.AreEqual(512, expressionEngine.Evaluate(expression));
        }
        [TestMethod]
        public void Test_003() {
            var expression = "(2^3)^2"; // => 8^2 => 64
            var expressionEngine = new ExpressionEngine();

            Assert.AreEqual(64, expressionEngine.Evaluate(expression));
        }
        [TestMethod]
        public void Test_004() {
            var expression = "2^3^2";   // => 2^(3^2) => 2^9 => 512
            var expressionEngine = new ExpressionEngine();

            Assert.AreEqual(512, expressionEngine.Evaluate(expression));
        }
        [TestMethod]
        public void Test_005() {
            var expression = "2^3!^2";  // => 2^((3*2)^2) => 2^(6^2) => 2^36 = 68719476736
            var expressionEngine = new ExpressionEngine();

            Assert.AreEqual(68719476736, expressionEngine.Evaluate(expression));
        }
        [TestMethod]
        public void Test_006() {
            var expression = "(2^3!)^2"; // => (2^(3*2))^2 => (2^6)^2 => 64^2 = 4096
            var expressionEngine = new ExpressionEngine();

            Assert.AreEqual(4096, expressionEngine.Evaluate(expression));
        }
    }
}
