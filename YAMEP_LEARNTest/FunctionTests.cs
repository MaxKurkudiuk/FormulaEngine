using Microsoft.VisualStudio.TestTools.UnitTesting;
using YAMEP_LEARN;

namespace YAMEP_LEARN.Tests {
    [TestClass]
    public class FunctionTests {
        private ExpressionEngine _expressionEngine;
        public FunctionTests() {
            _expressionEngine = new ExpressionEngine();
            _expressionEngine.AddFunctions<SupportedFunctions>();
        }

        [TestMethod()]
        public void Sin_Test_001() {
            var radian = 30;
            var expression = $"sin({radian})";
            Assert.AreEqual(System.Math.Sin(radian), _expressionEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void Cos_Test_001() {
            var radian = 30;
            var expression = $"Cos({radian})";
            Assert.AreEqual(System.Math.Cos(radian), _expressionEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void Tan_Test_001() {
            var radian = 30;
            var expression = $"Tan({radian})";
            Assert.AreEqual(System.Math.Tan(radian), _expressionEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void Tg_Test_001() {
            var radian = 30;
            var expression = $"Tg({radian})";
            Assert.AreEqual(System.Math.Tan(radian), _expressionEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void Ctg_Test_001() {
            var radian = 30;
            var expression = $"Ctg({radian})";
            Assert.AreEqual(1 / System.Math.Tan(radian), _expressionEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void Log_Test_001() {
            var num = 30;
            var expression = $"Log({num})";
            Assert.AreEqual(System.Math.Log(num), _expressionEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void Log_Test_002() {
            var num1 = 10;
            var num2 = 20;
            var expression = $"Log({num1}, {num2})";
            Assert.AreEqual(System.Math.Log(num1, num2), _expressionEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void Log10_Test_001() {
            var num = 30;
            var expression = $"Log10({num})";
            Assert.AreEqual(System.Math.Log10(num), _expressionEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void Pow_Test_001() {
            var num1 = 4;
            var num2 = 4;
            var expression = $"Pow({num1}, {num2})";
            Assert.AreEqual(System.Math.Pow(num1, num2), _expressionEngine.Evaluate(expression));
        }

        [TestMethod()]
        [ExpectedException(typeof(System.Exception), "Wrong count of parameters in Function Pow. Must be 2 arguments but not 1")]
        public void NotValidParameters_Test_001() {
            var num1 = 4;
            var expression = $"Pow({num1})";
            Assert.AreEqual(1, _expressionEngine.Evaluate(expression));
        }

        [TestMethod()]
        [ExpectedException(typeof(System.Exception), "Wrong count of parameters in Function Pow. Must be 1 arguments but not 2")]
        public void NotValidParameters_Test_002() {
            var num = 4;
            var expression = $"Sin({num}, {num})";
            Assert.AreEqual(1, _expressionEngine.Evaluate(expression));
        }
    }
}
