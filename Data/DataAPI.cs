using System;
using System.Collections.Generic;

namespace Data
{

    // Data Abstract API
    public abstract class DataAPI
    {
        public Area Area { get; set; }
        public int R { get; protected set; }
        public double MinSpeed { get; protected set; }
        public double MaxSpeed { get; protected set; }
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
        
        public static DataAPI CreateDataLayer(int width, int height, int r, double minSpeed, double maxSpeed)
        {
            return new DataLayer(width, height, r, minSpeed, maxSpeed);
        }
    }

    //  Concrete implementation of DataAPI abstract api
    internal class DataLayer : DataAPI
    {
        public DataLayer(int areaWidth, int areaHeight, int radius, double minSpeed, double maxSpeed)
        {
            Area = new Area(areaWidth, areaHeight);
            R = radius;
            MinSpeed = minSpeed;
            MaxSpeed = maxSpeed;
        }

        public override int Amount => Area.BallList.Count;

        public override void AddBalls(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Area.BallList.Add(new Ball(R, Area.Width, Area.Height, MinSpeed, MaxSpeed));
            }
        }
        public override void RemoveBalls(int count)
        {
            int startIndex = count > Amount ? 0 : Amount - count;
            int amountBeforeRemove = Amount;
            for (int i = startIndex; i < amountBeforeRemove; i++)
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
