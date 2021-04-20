using Microsoft.VisualStudio.TestTools.UnitTesting;
using YAMEP_LEARN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMEP_LEARN.Tests {
    [TestClass()]
    public class ExpressionEngineTests {
        [TestMethod()]
        public void EvaluateTest() {
            var expression = "1 + 2 * 3";

            var result = ExpressionEngine.Evaluate(expression);

            Assert.AreEqual(7, result);
        }
    }
} 