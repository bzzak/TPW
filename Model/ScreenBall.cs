using System;
using System.Collections.Generic;
using System.Text;
using Logic;

namespace Model
{
   
    public class ScreenBall
    {
        private readonly Data.Ball ball;

        public int Size { get { return ball.R * 2; } }
        public float Speed { get { return ball.Speed; } }
        public float X { get { return ball.Position.X; } }
        public float Y { get { return ball.Position.Y; } }

        //The same for direction the ball moves towards.
        public float Direction_X { get { return ball.Direction.X; } }
        public float Direction_Y { get { return ball.Direction.Y; } }



        public ScreenBall(Data.Ball b)
        {
            ball = b;
        }
    }
}
