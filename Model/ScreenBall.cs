using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Logic;

namespace Model
{
   
    public class ScreenBall : IBall
    {

        public float Size { get; }

        private float x;
        private float y;

        public event PropertyChangedEventHandler PropertyChanged;

        public float X
        {
            get { return x; }
            set
            {
                if (x == value) return;
                x = value;
                RaisePropertyChanged();
            }
        }
        public float Y
        {
            get { return y; }
            set
            {
                if (y == value) return;
                y = value;
                RaisePropertyChanged();
            }
        }

        //Constructor for visual representation of the ball.
        public ScreenBall(float x, float y, float radius)
        {
            this.x = x;
            this.y = y;
            Size = radius * 2;
        }

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
