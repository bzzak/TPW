using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Data
{
    internal enum Axis
    {
        X, Y
    }


    public class Ball
    {
        //private
        private float speed;
        // properties
        public Random Rand { get; } = new Random();
        public int R { get; }
        public Vector2 Position { get; private set; } = new Vector2();
        public Vector2 Direction { get; private set; }
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
            //randomize ball position  start with some distance from the edge of given area.
            int x = (R * 2) + Rand.Next(areaWidth - (R * 4));
            int y = (R * 2) + Rand.Next(areaHeight - (R * 4));
            Position = new Vector2(x, y);
        }

        // method to set randomized start ball direction
        private void SetRandomDirection()
        {
            //Draw vector turn
            int X_turn = Rand.Next(2) == 1 ? 1 : -1;
            int Y_turn = Rand.Next(2) == 1 ? 1 : -1;

            //Randomize direction vector
            float x = (float)(0.0001 * X_turn * (1 + Rand.Next(10000)));
            float y = (float)(0.0001 * Y_turn * (1 + Rand.Next(10000)));
            Vector2 temp = new Vector2(x, y);
            Direction = new Vector2(x / temp.Length(), y / temp.Length());
        }

        // method to set random speed in given range
        private void SetRandomSpeed(double minSpeed, double maxSpeed)
        {
            double val = (Rand.NextDouble() * (maxSpeed - minSpeed) + minSpeed);
            Speed = (float)val;
        }

        // method to chang ball position vactor turn when hit area boundaries
        private void ReflectDirection(Axis axis)
        {
            if (axis == Axis.X) Direction = new Vector2(-Direction.X, Direction.Y);
            else Direction = new Vector2(Direction.X, -Direction.Y);
        }

        // method to move ball with seted speed towards seted direction
        public void Move(int areaWidth, int areaHeight)
        {
            //If the ball is about to hit the edge of the area, we reflect the direction (hight, width - upper boudries, 0 - lower boundries).
            if (Position.X + (R * 2) + (Direction.X * Speed) > areaWidth || Position.X + (Direction.X * Speed) < 0) ReflectDirection(Axis.X);
            if (Position.Y + (R * 2) + (Direction.Y * Speed) > areaHeight || Position.Y + (Direction.Y * Speed) < 0) ReflectDirection(Axis.Y);

            //move some distance toward given direction that is increased by given speed.
            Position += Direction * Speed;
        }

    }
}
