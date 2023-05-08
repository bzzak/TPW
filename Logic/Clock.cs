using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;

namespace Logic
{
    public abstract class ClockAPI
    {
        public abstract event EventHandler Tick;
        public abstract TimeSpan Interval { get; set; }
        public abstract void Start();
        public abstract void Stop();
        public static ClockAPI CreateClock()
        {
            return new Clock();
        }
    }
    internal class Clock : ClockAPI
    {
        private readonly DispatcherTimer clock;

        public Clock()
        {
            clock = new DispatcherTimer();
        }

        public override TimeSpan Interval { get => clock.Interval; set => clock.Interval = value; }

        public override event EventHandler Tick { add => clock.Tick += value; remove => clock.Tick -= value; }
        public override void Start()
        {
            clock.Start();
        }

        public override void Stop()
        {
            clock.Stop();
        }
        
    }
}
