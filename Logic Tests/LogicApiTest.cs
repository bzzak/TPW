using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic;

namespace Logic_Tests
{
    [TestClass]
    public class LogicApiTest
    {
        //private readonly LogicAPI apiNoParam = LogicAPI.CreateLogicLayer();

        [TestMethod]
        public void LayerConstructionNonParametersTest()
        {
            LogicAPI apiNoParam = LogicAPI.CreateLogicLayer();
            Assert.AreEqual(apiNoParam.BallsAmount, 0);
            Assert.AreNotEqual(apiNoParam.AreaWidth, 0);
            Assert.AreNotEqual(apiNoParam.AreaHeight, 0);
        }

        [TestMethod]
        public void LayerConstructionParametersTest()
        {
            LogicAPI api = LogicAPI.CreateLogicLayer(100, 100, 2.0, 7.0);
            Assert.AreEqual(api.BallsAmount, 0);
            Assert.AreEqual(api.AreaWidth, 100);
            Assert.AreEqual(api.AreaHeight, 100);
        }

        [TestMethod]
        public void AdditionTest()
        {
            LogicAPI apiNoParam = LogicAPI.CreateLogicLayer();
            for (int i = 0; i < 10; i++)
            {
                apiNoParam.AddBall();
            }
            
            Assert.AreEqual(apiNoParam.BallsAmount, 10);
            for (int i = 0; i < 10; i++)
            {
                apiNoParam.AddBall();
            }
            Assert.AreEqual(apiNoParam.BallsAmount, 20);
            for (int i = 0; i < -50; i++)
            {
                apiNoParam.AddBall();
            }
            Assert.AreEqual(apiNoParam.BallsAmount, 20);
            for (int i = 0; i < 490; i++)
            {
                apiNoParam.AddBall();
            }
            Assert.AreEqual(apiNoParam.BallsAmount, 510);
            for (int i = 0; i < 1; i++)
            {
                apiNoParam.AddBall();
            }
            Assert.AreEqual(apiNoParam.BallsAmount, 511);
        }

        //[TestMethod]
        //public void RemovalTest()
        //{
        //    LogicAPI apiNoParam = LogicAPI.CreateLogicLayer();
        //    apiNoParam.RemoveAllBalls();
        //    Assert.AreEqual(apiNoParam.BallsAmount, 0);
        //    apiNoParam.AddBalls(10);
        //    apiNoParam.RemoveBalls(3);
        //    Assert.AreEqual(apiNoParam.BallsAmount, 7);
        //    apiNoParam.RemoveBalls(-3);
        //    Assert.AreEqual(apiNoParam.BallsAmount, 7);
        //    apiNoParam.RemoveBalls(20);
        //    Assert.AreEqual(apiNoParam.BallsAmount, 0);
        //}

        //[TestMethod]
        //public void SpeedTest()
        //{
        //    LogicAPI apiNoParam = LogicAPI.CreateLogicLayer();
        //    apiNoParam.AddBalls(2);
        //    float minSpeed = apiNoParam.BallsMinSpeed;
        //    float maxSpeed = apiNoParam.BallsMaxSpeed;
        //    float speedBall1 = apiNoParam.GetSpeed(0);
        //    float speedBall2 = apiNoParam.GetSpeed(1);
        //    apiNoParam.ChangeSpeed(true, 1);
        //    Assert.IsTrue(apiNoParam.GetSpeed(0) <= speedBall1 + 1.00001 && apiNoParam.GetSpeed(0) >= speedBall1 - 0.99999);
        //    Assert.IsTrue(apiNoParam.GetSpeed(1) <= speedBall2 + 1.00001 && apiNoParam.GetSpeed(1) >= speedBall2 - 0.99999);
        //    Assert.IsFalse(apiNoParam.ChangeSpeed(true, 30));
        //    apiNoParam.ChangeSpeed(false, 1);
        //    Assert.IsTrue(apiNoParam.GetSpeed(0) <= speedBall1 + 0.00001 && apiNoParam.GetSpeed(0) >= speedBall1 - 0.00001);
        //    Assert.IsTrue(apiNoParam.GetSpeed(1) <= speedBall2 + 0.00001 && apiNoParam.GetSpeed(1) >= speedBall2 - 0.00001);
        //    Assert.IsFalse(apiNoParam.ChangeSpeed(false, 10));
        //    Assert.IsFalse(apiNoParam.ChangeSpeed(true, -10));
        //    Assert.IsFalse(apiNoParam.ChangeSpeed(false, -5));
        //}

    }
}
