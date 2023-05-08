using System;
using System.Collections.Generic;
using Data;

namespace Logic
{
    public abstract class LogicAPI
    {
        public abstract int BallsAmount { get; }

        public abstract event EventHandler Update;
        public abstract void AddBalls(int amount);
        public abstract void RemoveBalls(int amount);
        public abstract void RemoveAllBalls(int amount);
        public abstract void UpdateArea();
        public abstract void Start();
        public abstract void Stop();
        public abstract void SetInterval(int ms);
        public abstract float GetX(int i);
        public abstract float GetY(int i);
        public abstract float GetDirX(int i);
        public abstract float GetDirY(int i);
        public abstract float GetSpeed(int i);
        public abstract int GetRadius(int i);
        public abstract List<Ball> GetAllBalls();
        public abstract bool ChangeSpeed(bool change, float amount);


        public static LogicAPI CreateLogicLayer(DataAPI data = default, ClockAPI simulationClock = default(ClockAPI))
        {
            return new LogicLayer(data ?? DataAPI.CreateDataLayer(200, 200), simulationClock);
        }

        public static LogicAPI CreateLogicLayer(int width, int height, ClockAPI simulationClock = default(ClockAPI))
        {
            return new LogicLayer(DataAPI.CreateDataLayer(width, height), simulationClock);
        }
    }

    internal class LogicLayer : LogicAPI
    {
        private readonly ClockAPI clock;

        private readonly DataAPI dataLayer;

        public override event EventHandler Update { add => clock.Tick += value; remove => clock.Tick -= value; }

        public override int BallsAmount => dataLayer.GetBallsList().Count;
        public LogicLayer(DataAPI api, ClockAPI simulationClock)
        {
            dataLayer = api;
            clock = simulationClock;
            SetInterval(30);
            clock.Tick += (sender, args) => UpdateArea();
        }

        public override void AddBalls(int amount)
        {
            dataLayer.AddBalls(amount);
        }

        public override void RemoveBalls(int amount)
        {
            dataLayer.RemoveBalls(amount);
        }

        public override void RemoveAllBalls(int amount)
        {
            dataLayer.ClearArea();
        }

        public override void UpdateArea()
        {
            dataLayer.Area.MoveAll();
        }

        public override void Start()
        {
            clock.Start();
        }

        public override void Stop()
        {
            clock.Stop();
        }

        public override void SetInterval(int ms)
        {
            clock.Interval = TimeSpan.FromMilliseconds(ms);
        }

        public override float GetX(int i)
        {
           return dataLayer.GetX(i);
        }

        public override float GetY(int i)
        {
            return dataLayer.GetY(i);
        }

        public override float GetDirX(int i)
        {
            return dataLayer.GetDirX(i);
        }

        public override float GetDirY(int i)
        {
            return dataLayer.GetDirY(i);
        }

        public override float GetSpeed(int i)
        {
            return dataLayer.GetSpeed(i);
        }
        public override int GetRadius(int i)
        {
            return dataLayer.GetRadius(i);
        }
        public override List<Ball> GetAllBalls()
        {
            return dataLayer.Area.BallList;
        }

        public override bool ChangeSpeed(bool change, float amount)
        {
            bool inc = true;
            bool dec = true;
            for (int i = 0; i < BallsAmount; i++)
            {
                if (GetAllBalls()[i].Speed <= 0.5) dec = false;
                if (GetAllBalls()[i].Speed >= 30) inc = false;
            }
            if ((change && !inc) || (!change && !dec))
            {
                return false;
            }
            for (int i = 0; i < BallsAmount; i++)
            {
                if (change && inc)
                {
                    GetAllBalls()[i].Speed += amount;
                }
                else if (dec)
                {
                    GetAllBalls()[i].Speed -= amount;
                }
            }
            return true;
        }
    }
}
