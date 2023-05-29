using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;

namespace Data_Tests
{
    [TestClass]
    public class DataApiTest
    {
        private readonly DataAPI data = DataAPI.CreateDataLayer(100, 100, 3.0, 7.0); 
        
        [TestMethod]
        public void ConstructorTest()
        {
            Assert.AreEqual(data.GetWidth(), 100);
            Assert.AreEqual(data.GetHeight(), 100);
            Assert.AreEqual(data.MinSpeed, 3.0);
            Assert.AreEqual(data.MaxSpeed, 7.0);
        }

        [TestMethod]
        public void AdditionTest()
        {

            for (int i = 0; i < 100; i++)
            {
                data.AddBall();
            }
            Assert.AreEqual(data.Amount, 100);
            for (int i = 0; i < 200; i++)
            {
                data.AddBall();
            }
            Assert.AreEqual(data.Amount, 300);
        }

        //[TestMethod]
        //public void RemovalTest()
        //{
        //    for (int i = 0; i < 100; i++)
        //    {
        //        data.AddBall();
        //    }
        //    for (int i = 0; i < 200; i++)
        //    {
        //        data.RemoveBall();
        //    }
        //    Assert.AreEqual(data.Amount, 200);

        //    for (int i = 0; i < 200; i++)
        //    {
        //        data.RemoveBall();
        //    }
        //    Assert.AreEqual(data.Amount, 0);
        //}

        [TestMethod]
        public void ClearTest()
        {
            for (int i = 0; i < 10; i++)
            { 
                data.AddBall();
            }
            data.ClearArea();
            Assert.AreEqual(data.Amount, 0);
        }


    }
}

