using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YAMEP_LEARN.Tests {
    [TestClass]
    public class SymbolTableTests {

        [TestMethod()]
        public void AddOrUpdateTest() {
            var variables = new { x = 5, y = 10 };
            var st = new SymbolTable();

            st.AddOrUpdate(variables);

            Assert.IsNotNull(st.Get("x"));
            Assert.IsNotNull(st.Get("y"));

            Assert.AreEqual(5, (st.Get("x")[0] as VariableSymbolTableEntry).Value);
            Assert.AreEqual(10, (st.Get("y")[0] as VariableSymbolTableEntry).Value);
        }

        [TestMethod()]
        public void NotRequiredMethods() {
            var st = new SymbolTable();
            st.AddFunctions<SupportedFunctions>();

            Assert.AreEqual(null, st.Get("Not_Me_1"));
            Assert.AreEqual(null, st.Get("Not_Me_2"));
            Assert.AreEqual(null, st.Get("Not_Me_3"));
        }
    }
}
