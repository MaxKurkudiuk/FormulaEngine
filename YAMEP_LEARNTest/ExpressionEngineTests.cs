using Microsoft.VisualStudio.TestTools.UnitTesting;
using YAMEP_LEARN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMEP_LEARN.Tests {
    [TestClass()]
    public class ExpressionEngineTests : UnitTestBase{

        [TestMethod()]
        public void Negate_Test_001() {
            var expression = "- 3 * (1 + 2)"; //-9

            var evalEngine = new ExpressionEngine();

            Assert.AreEqual(-9, evalEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void SunExpression_Test_001() {
            var expression = "(1 + 2) * 3"; //9

            var evalEngine = new ExpressionEngine();

            Assert.AreEqual(9, evalEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void FP_Test_001() {
            var expression = "1 + 1e5";

            var evalEngine = new ExpressionEngine();

            Assert.AreEqual(100001, evalEngine.Evaluate(expression));
        }

        [TestMethod()]
        public void EvaluateTest_001() {
            var evalEngine = new ExpressionEngine();

            (string, int)[] tests = new (string, int)[] {
                ("1 + 2", 3),
                ("2 * 3", 6),
                ("1 - 2", -1),
                ("2 / 2", 1),
                ("1 + 2 + 3", 6),
                ("10 - 9 + 9", 10),
                ("3 * 4 - 10", 2)
            };

            foreach (var (e, r) in tests)
                Assert.AreEqual(r, evalEngine.Evaluate(e));
        }
    }
} 