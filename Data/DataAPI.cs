using System;
using System.Collections.Generic;

namespace Data
{

    // Data Abstract API
    public abstract class DataAPI
    {
        public Area Area { get; set; }
        public abstract int Amount { get; }
        public abstract void AddBalls(int amount);
        public abstract void RemoveBalls(int amount);
        public abstract void ClearArea();
        public abstract List<Ball> GetBallsList();
        public abstract float GetX(int i);
        public abstract float GetY(int i);
        public abstract float GetDirX(int i);
        public abstract float GetDirY(int i);
        public abstract float GetSpeed(int i);
        public abstract int GetRadius(int i);
        
        public static DataAPI CreateDataLayer(int width, int height)
        {
            return new DataLayer(width, height);
        }
    }

    //  Concrete implementation of DataAPI abstract api
    internal class DataLayer : DataAPI
    {
        public DataLayer(int areaWidth, int areaHeight)
        {
            Area = new Area(areaWidth, areaHeight);
        }

        public override int Amount => Area.BallList.Count;

        public override void AddBalls(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Area.BallList.Add(new Ball(30f, 30f, 5, 3.0f));
            }
        }
        public override void RemoveBalls(int count)
        {
            int startIndex = count > Amount ? 0 : Amount - count;
            for (int i = startIndex; i < Amount; i++)
            {
                Area.BallList.Remove(Area.BallList[Amount - 1]);
            }
        }
        public override void ClearArea()
        {
            Area.RemoveAll();
        }
        public override List<Ball> GetBallsList()
        {
            return Area.BallList;
        }
        public override float GetX(int i)
        {
            return i >= 0 && i <= Amount - 1 && Amount != 0 ? Area.BallList[i].Position.X : 0;
        }
        public override float GetY(int i)
        {
            return i >= 0 && i < Amount ? Area.BallList[i].Position.Y : 0;
        }
        public override float GetDirX(int i)
        {
            return i >= 0 && i < Amount ? Area.BallList[i].Direction.X : 0;
        }
        public override float GetDirY(int i)
        {
            return i >= 0 && i < Amount ? Area.BallList[i].Direction.Y : 0;
        }
        public override float GetSpeed(int i)
        {
            return i >= 0 && i < Amount ? Area.BallList[i].Speed : 0;
        }
        public override int GetRadius(int i)
        {
            return i >= 0 && i < Amount ? Area.BallList[i].R : 0;
        }

    }
}
