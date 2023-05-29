using Microsoft.VisualStudio.TestTools.UnitTesting;
using Data;

namespace Data_Tests
{
    [TestClass]
    public class AreaTest
    {
        [TestMethod]
        public void BaseConstructorTest()
        {
            Area a = new Area(100, 200);
            Assert.AreEqual(a.Width, 100);
            Assert.AreEqual(a.Height, 200);
            Assert.AreEqual(a.BallList.Count, 0);
        }

        [TestMethod]
        public void FillConstructorTest()
        {
            Area a = new Area(5, 100, 200);
            Assert.AreEqual(a.Width, 100);
            Assert.AreEqual(a.Height, 200);
            Assert.AreEqual(a.BallList.Count, 5);
        }

        [TestMethod]
        public void ModificationsTest()
        {
            Area a = new Area(100, 100);
            Ball[] balls = { new Ball(20.0f, 20.0f, 5, 3.0f), new Ball(20.0f, 20.0f, 5, 3.0f), new Ball(20.0f, 20.0f, 5, 3.0f), new Ball(20.0f, 20.0f, 5, 3.0f), new Ball(20.0f, 20.0f, 5, 3.0f) };
            for(int i = 0; i < 5; i++)
            {
                a.Add(balls[i]);
            }
            
            Assert.AreEqual(a.BallList.Count, 5);
            a.Remove(balls[4]);
            a.Remove(balls[3]);
            Assert.AreEqual(a.BallList.Count, 3);
            a.RemoveAll();
            Assert.AreEqual(a.BallList.Count, 0);
        }

        //[TestMethod]
        //public void MovingTest()
        //{
        //    Area a = new Area(100, 100);
        //    Ball[] balls = { new Ball(20.0f, 20.0f, 5, 3.0f), new Ball(20.0f, 20.0f, 5, 3.0f), new Ball(20.0f, 20.0f, 5, 3.0f), new Ball(20.0f, 20.0f, 5, 3.0f), new Ball(20.0f, 20.0f, 5, 3.0f) };
        //    for (int i = 0; i < 5; i++)
        //    {
        //        a.Add(balls[i]);
        //    }

        //    a.MoveAll();

        //    for (int i = 0; i < 5; i++)
        //    {
        //        Assert.AreNotEqual(balls[i].Position.X, 20);
        //        Assert.AreNotEqual(balls[i].Position.Y, 20);
        //    }
        //}


    }
}