using Microsoft.VisualStudio.TestTools.UnitTesting;
using YAMEP_LEARN;

namespace YAMEP_LEARN.Tests {
    [TestClass]
    public class FactorialTests {
        [TestMethod()]
        public void Test_001() {
            var expression = "5!"; //120

            var evalEngine = new ExpressionEngine();

            Assert.AreEqual(120, evalEngine.Evaluate(expression));
        }

        [TestMethod()]
        //[ExpectedException(typeof(System.Exception), "Factorial only supports Positive Integers")]
        public void Test_002() {
            var expression = "-5!"; //-120

            var evalEngine = new ExpressionEngine();

            Assert.AreEqual(-120, evalEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void Test_003() {
            var expression = "(2 + 3)!"; //120

            var evalEngine = new ExpressionEngine();

            Assert.AreEqual(120, evalEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void Test_004() {
            var expression = "-(2 + 3)!"; //-120

            var evalEngine = new ExpressionEngine();

            Assert.AreEqual(-120, evalEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void Test_005() {
            var expression = "0!"; //1

            var evalEngine = new ExpressionEngine();

            Assert.AreEqual(1, evalEngine.Evaluate(expression));
        }
    }
}
