using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace Data
{
    public enum Axis
    {
        X, Y
    }


    public class Ball : IObservable<Ball>
    {
        //private
        private float speed;
        private Task BallThread;

        // fields
        internal readonly IList<IObserver<Ball>> observers = new List<IObserver<Ball>>();

        // properties
        public int R { get; set; }
        public Vector2 Position { get; private set; } = new Vector2();
        public Vector2 Direction { get; set; }
        public float M
        {
            get => R / 2;
        }
        public float Speed
        {
            get => speed;

            set => speed = value <= 0.001 ? 0.001f : value;
        }

        // constructors
        public Ball(float x, float y, int r, float speed)
        {
            Position = new Vector2(x,y);
            R = r;
            Speed = speed;
            SetRandomDirection();
        }
        public Ball(float x, float y, int r, double minSpeed, double maxSpeed)
        {
            Position = new Vector2(x, y);
            R = r;
            SetRandomSpeed(minSpeed, maxSpeed);
            SetRandomDirection();
        }
        public Ball(int r, int areaWidth, int areaHeight, float speed)
        {
            R = r;
            Speed = speed;
            SetRandomPosition(areaWidth, areaHeight);
            SetRandomDirection();
        }
        public Ball(int r, int areaWidth, int areaHeight, double minSpeed, double maxSpeed)
        {
            R = r;
            SetRandomSpeed(minSpeed, maxSpeed);
            SetRandomPosition(areaWidth, areaHeight);
            SetRandomDirection();
        }

        // methods

        // method to set randomized start ball position based on given area boundaries
        private void SetRandomPosition(int areaWidth, int areaHeight)
        {
            Random randomiser = new Random();
            //randomize ball position  start with some distance from the edge of given area.
            int x = (R * 2) + randomiser.Next(areaWidth - (R * 4));
            int y = (R * 2) + randomiser.Next(areaHeight - (R * 4));
            Position = new Vector2(x, y);
        }

        // method to set randomized start ball direction
        private void SetRandomDirection()
        {
            Random randomiser = new Random();
            //Draw vector turn
            int X_turn = randomiser.Next(2) == 1 ? 1 : -1;
            int Y_turn = randomiser.Next(2) == 1 ? 1 : -1;

            //Randomize direction vector
            float x = (float)(0.0001 * X_turn * (1 + randomiser.Next(10000)));
            float y = (float)(0.0001 * Y_turn * (1 + randomiser.Next(10000)));
            Vector2 temp = new Vector2(x, y);
            Direction = new Vector2(x / temp.Length(), y / temp.Length());
        }

        // method to set random speed in given range
        private void SetRandomSpeed(double minSpeed, double maxSpeed)
        {
            Random randomiser = new Random();
            double val = (randomiser.NextDouble() * (maxSpeed - minSpeed) + minSpeed);
            Speed = (float)val;
        }

        // method to chang ball position vactor turn when hit area boundaries
        public void ReflectDirection(Axis axis)
        {
            if (axis == Axis.X) Direction = new Vector2(-Direction.X, Direction.Y);
            else Direction = new Vector2(Direction.X, -Direction.Y);

            NotifyObservers();
        }

        // method to move ball with seted speed towards seted direction
        public void Move()
        {
            //lock the whole object so that nothing can access it's properities while movement ongoing.
            lock (this)
            {
                //move some distance toward given direction that is increased by given speed.
                Position += Direction * Speed;
            }
            NotifyObservers();
        }


        //Method performed as task in loop. It executes movement (one tick) and notifies observers.
        private void MovingTask()
        {
            while (true) Move();
        }

        //When notified every observer receives update to data it's working on.
        public void NotifyObservers()
        {
            foreach (var observer in observers.ToList())
            {
                if (observer != null)
                {
                    observer.OnNext(this);
                }

            }
        }

        //If task not started, we create a task that is meant to move the ball.
        public void StartMoving()
        {
            if (BallThread is null)
            {
                CancellationToken token = new CancellationToken();
                this.BallThread = new Task(MovingTask, token);
            }
            BallThread.Start();
        }

        public IDisposable Subscribe(IObserver<Ball> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private IList<IObserver<Ball>> observers;
            private IObserver<Ball> observer;

            public Unsubscriber(IList<IObserver<Ball>> observers, IObserver<Ball> observer)
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

    }
}
