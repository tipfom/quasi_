using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Core.Test {
    [TestClass]
   public class MathiTest {
        [TestMethod]
        public void CeilTest ( ) {
            for(float i = -10; i < 10; i += 0.1f) {
                Assert.AreEqual(Math.Ceiling(i), Mathi.Ceil(i));
            }
        }

        [TestMethod]
        public void FloorTest ( ) {
            for (float i = -10; i < 10; i += 0.1f) {
                Assert.AreEqual(Math.Floor(i), Mathi.Floor(i));
            }
        }
    }
}
