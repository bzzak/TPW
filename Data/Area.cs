using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class Area
    {
        // properties
        public List<Ball> BallList { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        // constructors

        //Basic constructor that initialize empty list of balls
        public Area(int width, int height)
        {
            Width = width;
            Height = height;
            BallList = new List<Ball>();
        }

        //This constructor uses the basic one and creates required amount of spheres in fixed positions.
        public Area(int ballAmount, int width, int height) : this(width, height)
        {
            for (int i = 0; i < ballAmount; i++)
            {
                BallList.Add(new Ball(30f, 30f, 5, 3.0f));
            }
        }

        // methods

        // Adding new ball to the Area.
        public void Add(Ball b)
        {
            if (!(b is null))
            {
                BallList.Add(b);
            }
        }

        // Removing ball from the list.
        public void Remove(Ball b)
        {
            if (!(b is null))
            {
                BallList.Remove(b);
            }
        }

        // Clearing ball list.
        public void RemoveAll()
        {
            BallList.Clear();
        }

        // Invoke move method from all balls contained in ball list
        public void MoveAll()
        {
            for (int i = 0; i < BallList.Count; i++)
            {
                BallList[i].Move(Width, Height);
            }
        }
    }
}
