using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;

namespace Data_Tests
{
    [TestClass]
    public class BallTest
    {
        [TestMethod]
        public void BaseConstructorTest()
        {
            Ball b = new Ball(5.0f, 15.0f, 10, 3.0f);
            Assert.AreEqual(b.Position.X, 5);
            Assert.AreEqual(b.Position.Y, 15);
            Assert.AreEqual(b.R, 10);
            Assert.AreEqual(b.Speed, 3.0f);
            Assert.IsTrue(b.Direction.Length() <= 1.00001f && b.Direction.Length() >= 0.99999);
        }

        [TestMethod]
        public void RandomSpeedConstructorTest()
        {
            for (int i = 0; i < 100; i++)
            {
                Ball b = new Ball(5.0f, 15.0f, 10, 3.0, 7.0);
                Assert.AreEqual(b.Position.X, 5);
                Assert.AreEqual(b.Position.Y, 15);
                Assert.AreEqual(b.R, 10);
                Assert.IsTrue(b.Direction.Length() <= 1.00001f && b.Direction.Length() >= 0.99999);
                Assert.IsTrue(b.Speed <= 7.0f && b.Speed >= 3.0f);
            }
        }

        [TestMethod]
        public void RandomPositionConstructorTest()
        {
            for (int i = 0; i < 100; i++)
            {
                Ball b = new Ball(5, 100, 100, 3.0f);
                Assert.IsTrue(b.Position.X <= 100 - (2 * b.R) && b.Position.X >= 0);
                Assert.IsTrue(b.Position.Y <= 100 - (2 * b.R) && b.Position.Y >= 0);
                Assert.AreEqual(b.R, 5);
                Assert.AreEqual(b.Speed, 3.0f);
                Assert.IsTrue(b.Direction.Length() <= 1.00001f && b.Direction.Length() >= 0.99999);
            }
        }

        [TestMethod]
        public void RandomPositionAndSpeedConstructorTest()
        {
            for (int i = 0; i < 100; i++)
            {
                Ball b = new Ball(5, 100, 100, 3.0, 7.0);
                Assert.IsTrue(b.Position.X <= 100 - (2 * b.R) && b.Position.X >= 0);
                Assert.IsTrue(b.Position.Y <= 100 - (2 * b.R) && b.Position.Y >= 0);
                Assert.AreEqual(b.R, 5);
                Assert.IsTrue(b.Speed <= 7.0f && b.Speed >= 3.0f);
                Assert.IsTrue(b.Direction.Length() <= 1.00001f && b.Direction.Length() >= 0.99999);
            }
        }

        

        //[TestMethod]
        //public void MoveTest()
        //{
        //    Ball b1 = new Ball(5.0f, 15.0f, 10, 3.0f);
        //    b1.Move(100, 100);
        //    Assert.IsTrue(b1.Position.X <= 5.0f + (3.0f * b1.Direction.X) + 0.00001  && b1.Position.X >= 5.0f + (3.0f * b1.Direction.X) - 0.00001);
        //    Assert.IsTrue(b1.Position.Y <= 15.0f + (3.0f * b1.Direction.Y) + 0.00001 && b1.Position.Y >= 5.0f + (3.0f * b1.Direction.Y) - 0.00001);

        //    Ball b2 = new Ball(1.0f, 1.0f, 5, 3.0f);
        //    for (int i = 0; i < 100; i++)
        //    {
        //        b2.Move(25, 25);
        //        Assert.IsTrue(b2.Position.X <= 25 - (2 * b2.R) && b2.Position.X >= 0);
        //        Assert.IsTrue(b2.Position.Y <=  25 - (2 * b2.R) && b2.Position.Y >= 0);
        //    }
        //}
    }
}
