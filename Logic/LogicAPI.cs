using System;
using System.Collections.Generic;
using System.Numerics;
using Data;
using System.Timers;

namespace Logic
{
    // Logic Abstract API
    public abstract class LogicAPI : IObservable<int>, IObserver<Ball>
    {
        public float BallsMinSpeed { get; protected set; }
        public float BallsMaxSpeed { get; protected set; }
        public abstract int BallsAmount { get; }
        public abstract int AreaWidth { get; }
        public abstract int AreaHeight { get; }

        public abstract event EventHandler Update;
        public abstract void AddBall();
        public abstract bool RemoveBall();
        public abstract void RemoveAllBalls();
        public abstract void UpdateArea();
        public abstract void UpdateBall(int id);
        public abstract Vector2 GetPosition(int i);
        public abstract Vector2 GetDirection(int i);
        public abstract float GetSpeed(int i);
        public abstract int GetRadius(int i);
        public abstract List<Ball> GetAllBalls();
        public abstract bool ChangeSpeed(bool change, float amount);
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(Ball sphere);
        public abstract IDisposable Subscribe(IObserver<int> observer);


        public static LogicAPI CreateLogicLayer(float minSpeed = 0.5f, float maxSpeed = 30.0f, DataAPI data = default)
        {
            SortAscending(ref minSpeed, ref maxSpeed);
            return new LogicLayer(minSpeed, maxSpeed, data ?? DataAPI.CreateDataLayer(785, 265, 0.5, 1.0));
        }

        public static LogicAPI CreateLogicLayer(int width, int height, double minSpeedRandom, double maxSpeedRandom, float minSpeed = 0.5f, float maxSpeed = 30.0f)
        {
            CheckSpeedParameters(minSpeedRandom, maxSpeedRandom, minSpeed, maxSpeed);
            return new LogicLayer(minSpeed, maxSpeed, DataAPI.CreateDataLayer(width, height, minSpeedRandom, maxSpeedRandom));
        }
        private static void CheckSpeedParameters(double minSpeedRandom, double maxSpeedRandom, float minSpeed, float maxSpeed)
        {
            SortAscending(ref minSpeed, ref maxSpeed);
            SortAscending(ref minSpeedRandom, ref maxSpeedRandom);
            if (minSpeedRandom < minSpeed) minSpeedRandom = minSpeed;
            if (maxSpeedRandom > maxSpeed) maxSpeedRandom = maxSpeed;

        }
        private static void SortAscending<T>(ref T val1, ref T val2) where T : IComparable<T>
        {
            if (val2.CompareTo(val1) < 0)
            {
                T temp = val1;
                val1 = val2;
                val2 = temp;
            }
        }
    }
    //  Concrete implementation of LogicAPI abstract api
    internal class LogicLayer : LogicAPI
    {
        private readonly DataAPI dataLayer;
        private Data.Logger logger;
        private IDisposable unsubscriber;
        private IList<IObserver<int>> observers;

        public override event EventHandler Update;

        public override int BallsAmount => dataLayer.GetBallsList().Count;
        public override int AreaWidth => dataLayer.GetWidth();
        public override int AreaHeight => dataLayer.GetHeight();

        public LogicLayer(float minSpeed, float maxSpeed, DataAPI api)
        {
            dataLayer = api;
            Subscribe(dataLayer);
            observers = new List<IObserver<int>>();
            BallsMinSpeed = minSpeed;
            BallsMaxSpeed = maxSpeed;

            //create logger
            logger = new Data.Logger();
            logger.StartLogging();

            //create timer
            Timer timer = new Timer(3000);

            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        public override void AddBall()
        {
            dataLayer.AddBall();
        }

        public override bool RemoveBall()
        {
            return dataLayer.RemoveBall();
        }

        public override void RemoveAllBalls()
        {
            dataLayer.ClearArea();
        }

        public override void UpdateArea()
        {
            for (int i = 0; i < BallsAmount; i++)
            {
                dataLayer.StartMovingBall(i);
            }
        }
        public override void UpdateBall(int id)
        {
            if(id >= 0 && id < dataLayer.Amount) dataLayer.StartMovingBall(id);
        }

        public override Vector2 GetPosition(int i)
        {
           return dataLayer.GetPosition(i);
        }

        public override Vector2 GetDirection(int i)
        {
            return dataLayer.GetDirection(i);
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
            return dataLayer.GetBallsList();
        }

        public override bool ChangeSpeed(bool change, float amount)
        {
            if (amount >= 0)
            {
                bool inc = true;
                bool dec = true;

                for (int i = 0; i < BallsAmount; i++)
                {
                    if (change)
                    {
                        if (dataLayer.GetSpeed(i) + amount >= BallsMaxSpeed)
                        {
                            inc = false;
                            break;
                        }

                    }
                    else
                    {
                        if (dataLayer.GetSpeed(i) - amount <= BallsMinSpeed)
                        {
                            dec = false;
                            break;
                        }
                    }
                }

                if ((change && !inc) || (!change && !dec))
                {
                    return false;
                }

                for (int i = 0; i < BallsAmount; i++)
                {
                    if (change && inc)
                    {
                        dataLayer.SetSpeed(i, dataLayer.GetSpeed(i) + amount);
                    }
                    else if (dec)
                    {
                        dataLayer.SetSpeed(i, dataLayer.GetSpeed(i) - amount);
                    }
                }

                return true;
            }

            return false;
        }

        //Switches chosen direction of a sphere to oposite.
        private void SwitchDirection(int i, Axis axis)
        {
            dataLayer.SwitchDirection(i, axis);
            NotifyObservers(i);
        }

        //Checks for border colision of a sphere and, if necesseary, switches one of it's directions.
        private void CheckBoundries(int i)
        {
            Vector2 position = dataLayer.GetPosition(i);
            Vector2 direction = dataLayer.GetDirection(i);
            float speed = dataLayer.GetSpeed(i);
            int radius = dataLayer.GetRadius(i);

            if (position.X + radius * 2 + direction.X * speed > dataLayer.GetWidth() || position.X + direction.X * speed < 0)
                SwitchDirection(i, Axis.X);
            if (position.Y + radius * 2 + direction.Y * speed > dataLayer.GetHeight() || position.Y + direction.Y * speed < 0)
                SwitchDirection(i, Axis.Y);
        }

        //Finds distance between current sphere and other spheres. If distance is lower or equal
        //to sum of this and other sphere's radius - they collide!
        private void CheckCollisions(int id)
        {
            for (int i = 0; i < dataLayer.Amount; i++)
            {
                if (i == id) continue;
                double distance = Math.Sqrt(Math.Pow((dataLayer.GetPosition(id).X + dataLayer.GetDirection(id).X * dataLayer.GetSpeed(id)) - (dataLayer.GetPosition(i).X
                                + dataLayer.GetDirection(i).X * dataLayer.GetSpeed(i)), 2) + Math.Pow((dataLayer.GetPosition(id).Y + dataLayer.GetDirection(id).Y * dataLayer.GetSpeed(id))
                                - (dataLayer.GetPosition(i).Y + dataLayer.GetDirection(i).Y * dataLayer.GetSpeed(i)), 2));

                if (Math.Abs(distance) <= dataLayer.GetRadius(id) + dataLayer.GetRadius(i))
                {
                    //When collision happens  assign new movement as a result of executing NewMovement() method.
                    float[] newMovement = NewMovement(id, i);
                    dataLayer.SetDirection(id, newMovement[0], newMovement[1]);
                    dataLayer.SetDirection(i, newMovement[2], newMovement[3]);
                }
            }

        }

        #region observer

        public virtual void Subscribe(IObservable<Ball> provider)
        {
            if (provider != null)
                unsubscriber = provider.Subscribe(this);
        }

        public override void OnCompleted()
        {
            unsubscriber.Dispose();
        }

        public override void OnError(Exception error)
        {
            throw error;
        }
        public void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            for (int i = 0; i < dataLayer.Amount; i++)
            {
                logger.AddLog(dataLayer.Log(i));
            }
        }
        public override void OnNext(Ball ball)
        {
            if (dataLayer.GetBallsList().Contains(ball))
            {
                int id = dataLayer.GetBallsList().IndexOf(ball);

                //for every sphere check for border collision, collision with other balls and then notify observers.
                CheckBoundries(id);
                CheckCollisions(id);
                NotifyObservers(id);
            }
        }

        //notify all observers
        public void NotifyObservers(int id)
        {
            foreach (var observer in observers)
            {
                if (observer != null)
                {
                    observer.OnNext(id);
                }
            }
            System.Threading.Thread.Sleep(1);

        }

        //calculating new vector of movement for two balls that collided. It takes speed, position of balls and mass into account.
        public float[] NewMovement(int id1, int id2)
        {
            float mass1 = dataLayer.GetMass(id1);
            float mass2 = dataLayer.GetMass(id2);

            float speed1 = dataLayer.GetSpeed(id1);
            float speed2 = dataLayer.GetSpeed(id2);

            Vector2 velocity1 = dataLayer.GetDirection(id1) * speed1;
            Vector2 position1 = dataLayer.GetPosition(id1);

            Vector2 velocity2 = dataLayer.GetDirection(id2) * speed2;
            Vector2 position2 = dataLayer.GetPosition(id2);

            float fDistance = (float)Math.Sqrt((position1.X - position2.X) * (position1.X - position2.X)
                   + (position1.Y - position2.Y) * (position1.Y - position2.Y));

            float nx = (position2.X - position1.X) / fDistance;
            float ny = (position2.Y - position1.Y) / fDistance;

            float tx = -ny;
            float ty = nx;

            // Dot Product Tangent
            float dpTan1 = velocity1.X * tx + velocity1.Y * ty;
            float dpTan2 = velocity2.X * tx + velocity2.Y * ty;

            // Dot Product Normal
            float dpNorm1 = velocity1.X * nx + velocity1.Y * ny;
            float dpNorm2 = velocity2.X * nx + velocity2.Y * ny;

            // Conservation of momentum in 1D
            float m1 = (dpNorm1 * (mass1 - mass2) + 2.0f * mass2 * dpNorm2) / (mass1 + mass2);
            float m2 = (dpNorm2 * (mass2 - mass1) + 2.0f * mass1 * dpNorm1) / (mass1 + mass2);

            float[] newMovements = new float[4]
            {
                    tx * dpTan1 + nx * m1,
                    ty * dpTan1 + ny * m1,
                    tx * dpTan2 + nx * m2,
                    ty * dpTan2 + ny * m2
            };

            return newMovements;
        }

        #endregion

        #region provider

        public override IDisposable Subscribe(IObserver<int> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private IList<IObserver<int>> _observers;
            private IObserver<int> _observer;

            public Unsubscriber
            (IList<IObserver<int>> observers, IObserver<int> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        #endregion
    }
}
