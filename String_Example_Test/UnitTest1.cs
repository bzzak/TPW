using Microsoft.VisualStudio.TestTools.UnitTesting;
using String_Example;

namespace String_Example_Test
{
    [TestClass]
    public class MyStringTest
    {

        [TestMethod]
        public void TestConcatText()
        {
            var str = new MyString("Ala");
            str.ConcatText("Ma", "Kota");
            Assert.AreEqual("AlaMaKota", str.GetText());

        }

        [TestMethod]
        public void TestToLower()
        {
            var str = new MyString("Ala");
            str.ToLower();
            Assert.AreEqual("ala", str.GetText());
        }

        [TestMethod]
        public void TestToUpper()
        {
            var str = new MyString("Ala");
            str.ToUpper();
            Assert.AreEqual("ALA", str.GetText());
        }
    }
}
