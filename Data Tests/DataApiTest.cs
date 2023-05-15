using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;

namespace Data_Tests
{
    [TestClass]
    public class DataApiTest
    {
        private readonly DataAPI data = DataAPI.CreateDataLayer(100, 100, 5, 3.0, 7.0); 
        
        [TestMethod]
        public void ConstructorTest()
        {
            Assert.AreEqual(data.Area.Width, 100);
            Assert.AreEqual(data.Area.Height, 100);
            Assert.AreEqual(data.R, 5);
            Assert.AreEqual(data.MinSpeed, 3.0);
            Assert.AreEqual(data.MaxSpeed, 7.0);
        }

        [TestMethod]
        public void AdditionTest()
        {
            data.AddBalls(100);
            Assert.AreEqual(data.Amount, 100);
            data.AddBalls(700);
            Assert.AreEqual(data.Amount, 500);
        }
        
        [TestMethod]
        public void RemovalTest()
        {
            data.AddBalls(500);
            data.RemoveBalls(300);
            Assert.AreEqual(data.Amount, 200);
            data.RemoveBalls(500);
            Assert.AreEqual(data.Amount, 0);
        }

        [TestMethod]
        public void ClearTest()
        {
            data.AddBalls(300);
            data.ClearArea();
            Assert.AreEqual(data.Amount, 0);
        }


    }
}

