using System;
using System.Collections.Generic;
using Logic;

namespace Model
{
    // Model Abstract API
    public abstract class ModelAPI
    {
        public int CanvasWidth { get; set; }
        public int CanvasHeight { get; set; }

        public List<ScreenBall> ScreenBalls;
        
        public int R { get; set; }

        public abstract void AddScreenBalls(int amount);
        public abstract void RemoveScreenBalls(int amount);
        public abstract void Start();
        public abstract void Stop();
        public abstract bool ChangeSpeed(bool b, float amount);

        public static ModelAPI CreateModelLayer(LogicAPI logic = default)
        {
            return new ModelLayer(logic ?? LogicAPI.CreateLogicLayer());
        }

        public static ModelAPI CreateModelLayer(int width, int height, int r, double minSpeed, double maxSpeed, ClockAPI simulationClock = default)
        {
            return new ModelLayer(LogicAPI.CreateLogicLayer(width, height, r, minSpeed, maxSpeed, simulationClock));
        }
    }
    //  Concrete implementation of ModelAPI abstract api
    internal class ModelLayer : ModelAPI
    {
        private readonly LogicAPI logicLayer;

        public ModelLayer(LogicAPI logic)
        {
            R = 25;
            logicLayer = logic;
            
            CanvasWidth = logicLayer.AreaWidth;
            CanvasHeight = logicLayer.AreaHeight;
            
            ScreenBalls = new List<ScreenBall>();

            ScreenBallsRefresh();
        }

        public override void AddScreenBalls(int amount)
        {
            logicLayer.AddBalls(amount);
            ScreenBallsRefresh();

        }

        public override void RemoveScreenBalls(int amount)
        {
            logicLayer.RemoveBalls(amount);
            ScreenBallsRefresh();
        }

        public override bool ChangeSpeed(bool b, float amount)
        {
            bool success = logicLayer.ChangeSpeed(b, amount);
            if (success)
            {
                ScreenBallsRefresh();
            }

            return success;
        }

        public override void Start()
        {
            logicLayer.Start();
        }

        public override void Stop()
        {
            logicLayer.Stop();
        }

        private void ScreenBallsRefresh()
        {
            lock (ScreenBalls)
            {
                ScreenBalls.Clear();
                lock (logicLayer.GetAllBalls())
                {
                    for (int i = 0; i < logicLayer.BallsAmount; i++)
                    {
                        ScreenBalls.Add(new ScreenBall(logicLayer.GetAllBalls()[i]));
                    }
                }
            }
        }
    }
}