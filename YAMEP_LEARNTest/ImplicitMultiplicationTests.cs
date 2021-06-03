using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using YAMEP_LEARN;

namespace YAMEP_LEARN.Tests {
    [TestClass()]
    public class ImplicitMultiplication {

        [TestMethod()]
        public void Test_000() {
            var expression = "2 x";
            var variables = new { x = 5 };
            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression, variables);

            Assert.AreEqual(10, result);
        }

        [TestMethod()]
        public void Test_001() {
            var expression = "2x";
            var variables = new { x = 5};
            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression, variables);

            Assert.AreEqual(10, result);
        }

        [TestMethod()]
        public void Test_002() {
            var expression = "2x + 3y";
            var variables = new { x = 5, y = 10 };
            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression, variables);

            Assert.AreEqual(40, result);
        }

        [TestMethod()]
        public void Test_003() {
            var expression = "2x(x+2y)";
            var variables = new { x = 5, y = 10 };
            var ee = new ExpressionEngine();
            var result = ee.Evaluate(expression, variables);

            Assert.AreEqual(250, result);
        }
    }
}
