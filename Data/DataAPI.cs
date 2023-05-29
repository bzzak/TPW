using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace Data 
{

    // Data Abstract API
    public abstract class DataAPI : IObserver<Ball>, IObservable<Ball>
    {
        public double MinSpeed { get; protected set; } = 0.0;
        public double MaxSpeed { get; protected set; } = 0.0;
        public abstract int Amount { get; }
        public abstract int GetHeight();
        public abstract int GetWidth();
        public abstract void AddBall();
        public abstract bool RemoveBall();
        public abstract void ClearArea();
        public abstract List<Ball> GetBallsList();
        public abstract Vector2 GetPosition(int i);
        public abstract Vector2 GetDirection(int i);
        public abstract void SetDirection(int i, float x, float y);
        public abstract float GetSpeed(int i);
        public abstract void SetSpeed(int i, float s);
        public abstract int GetRadius(int i);
        public abstract float GetMass(int i);
        public abstract void SwitchDirection(int i, Axis a);
        public abstract void StartMovingBall(int i);
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(Ball ball);
        public abstract IDisposable Subscribe(IObserver<Ball> observer);

        public static DataAPI CreateDataLayer(int width, int height, double minSpeed, double maxSpeed)
        {
            return new DataLayer(width, height, minSpeed, maxSpeed);
        }
    }

    //  Concrete implementation of DataAPI abstract api
    internal class DataLayer : DataAPI
    {
        //DataAPI will contains an area of balls  with some particular size.
        private readonly Area area;

        //Unsubscriber and observers.
        private IDisposable unsubscriber;
        private readonly IList<IObserver<Ball>> observers = new List<IObserver<Ball>>();

        public DataLayer(int areaWidth, int areaHeight, double minSpeed, double maxSpeed)
        {
            area = new Area(areaWidth, areaHeight);
            MinSpeed = minSpeed;
            MaxSpeed = maxSpeed;
        }

        public override int Amount => area.BallList.Count;
        public override int GetWidth()
        {
            return area.Width;
        }
        public override int GetHeight()
        {
            return area.Height;
        }

        public override void AddBall()
        {
            Random rnd = new Random();
            int newValue = 5 + rnd.Next(5);
           
            area.BallList.Add(new Ball(newValue, area.Width, area.Height, MinSpeed, MaxSpeed));
            Subscribe(area.BallList[Amount - 1]);
        }
        public override bool RemoveBall()
        {
            bool success;
            if (Amount > 0) success = area.BallList.Remove(area.BallList[Amount - 1]);
            else success = false;

            return success;
        }
        public override void ClearArea()
        {
            area.RemoveAll();
        }
        public override List<Ball> GetBallsList()
        {
            return area.BallList;
        }
        public override Vector2 GetPosition(int i)
        {
            return i >= 0 && i <= Amount - 1 && Amount != 0 ? area.BallList[i].Position : new Vector2(0,0);
        }
        public override Vector2 GetDirection(int i)
        {
            return i >= 0 && i < Amount ? area.BallList[i].Direction : new Vector2(0,0);
        }
        public override void SetDirection(int i, float x, float y)
        {
            area.BallList[i].Direction = new Vector2(x, y);
        }
        public override float GetSpeed(int i)
        {
            return i >= 0 && i < Amount ? area.BallList[i].Speed : 0;
        }
        public override void SetSpeed(int i, float s)
        {
            area.BallList[i].Speed = s;
        }
        public override int GetRadius(int i)
        {
            return i >= 0 && i < Amount ? area.BallList[i].R : 0;
        }
        public override float GetMass(int i)
        {
            return i >= 0 && i < Amount ? area.BallList[i].M : 0;
        }
        public override void SwitchDirection(int i, Axis a)
        {
            area.BallList[i].ReflectDirection(a);
        }
        public override void StartMovingBall(int i)
        {
            area.BallList[i].StartMoving();
        }

        #region Observer
        public override void OnCompleted()
        {
            unsubscriber.Dispose();
        }
        public override void OnError(Exception error)
        {
            throw error;
        }
        public override void OnNext(Ball ball)
        {
            foreach (IObserver<Ball> observer in observers)
            {
                observer.OnNext(ball);
            }
        }
        public virtual void Subscribe(IObservable<Ball> provider)
        {
            if (provider != null)
                unsubscriber = provider.Subscribe(this);
        }
        #endregion

        #region provider

        public override IDisposable Subscribe(IObserver<Ball> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly IList<IObserver<Ball>> observers;
            private readonly IObserver<Ball> observer;

            public Unsubscriber
            (IList<IObserver<Ball>> observers, IObserver<Ball> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (observer != null && observers.Contains(observer))
                    observers.Remove(observer);
            }
        }

        #endregion

    }
}
