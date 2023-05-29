using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Reactive;
using System.Reactive.Linq;
using Logic;

namespace Model 
{
    // Model Abstract API
    public abstract class ModelAPI : IObserver<int>, IObservable<IBall>
    {
        public int CanvasWidth { get; set; }
        public int CanvasHeight { get; set; }

        public List<ScreenBall> ScreenBalls;
        
        public int R { get; set; }

        public abstract void AddPresentBalls();
        public abstract void AddBalls(int amount);
        public abstract void RemoveBalls(int amount);
        public abstract void MoveAllBalls();
        public abstract void MoveBall(int id);
        public abstract bool ChangeSpeed(bool b, float amount);
        public abstract void Subscribe(IObservable<int> provider);
        public abstract void OnCompleted();
        public abstract void OnError(Exception error);
        public abstract void OnNext(int value);
        public abstract IDisposable Subscribe(IObserver<IBall> observer);

        public static ModelAPI CreateModelLayer(LogicAPI logic = default)
        {
            return new ModelLayer(logic ?? LogicAPI.CreateLogicLayer());
        }

        public static ModelAPI CreateModelLayer(int width, int height, double minSpeedRandom, double maxSpeedRandom, float minSpeed = 0.5f, float maxSpeed = 30.0f)
        {
            return new ModelLayer(LogicAPI.CreateLogicLayer(width, height, minSpeedRandom, maxSpeedRandom, minSpeed, maxSpeed));
        }
    }

    //Interface for IBall (model representation of ball, that notifies when something changed with their property).
    public interface IBall : INotifyPropertyChanged
    {
        float X { get; }
        float Y { get; }
        float Size { get; }
    }

    public class BallChangeEventArgs : EventArgs
    {
        public IBall Ball { get; set; }
    }

    public interface INotifyBallChanged
    {
        // Occurs when a property value changes.In this case: Sphere.
        event EventHandler<BallChangeEventArgs> BallChanged;
    }

    //  Concrete implementation of ModelAPI abstract api
    internal class ModelLayer : ModelAPI
    {
        private readonly LogicAPI logicLayer;

        //All those things necesseary to observe model layer and event handler for it.
        private IDisposable unsubscriber;
        public event EventHandler<BallChangeEventArgs> BallChanged;
        private IObservable<EventPattern<BallChangeEventArgs>> eventObservable = null;

        public ModelLayer(LogicAPI logic)
        {
            eventObservable = Observable.FromEventPattern<BallChangeEventArgs>(this, "BallChanged");
            logicLayer = logic;
            R = 5;

            CanvasWidth = logicLayer.AreaWidth;
            CanvasHeight = logicLayer.AreaHeight;
            
            ScreenBalls = new List<ScreenBall>();
            AddPresentBalls();
            Subscribe(logicLayer);
        }

        //Method for adding particular amount of spheres to the presentation layer.
        public override void AddPresentBalls()
        {
            for (int i = 0; i < logicLayer.BallsAmount; i++)
            {
                AddScreenBall(i);
            }
        }

        public override void AddBalls(int amount)
        {
            if (logicLayer.BallsAmount + amount > 500) amount = 500 - logicLayer.BallsAmount;

            for (int i = 0; i < amount; i++)
            {
                logicLayer.AddBall();
                AddScreenBall(logicLayer.BallsAmount - 1);
            }

            foreach (ScreenBall ball in ScreenBalls)
            {
                BallChanged?.Invoke(this, new BallChangeEventArgs() { Ball = ball });
            }
        }

        public override void RemoveBalls(int amount)
        {
            int startIndex = amount > logicLayer.BallsAmount ? 0 : logicLayer.BallsAmount - amount;
            int amountBeforeRemove = logicLayer.BallsAmount;

            for (int i = startIndex; i < amountBeforeRemove; i++)
            {
                bool success = logicLayer.RemoveBall();
                if(success) RemoveScreenBall();
            }

            foreach (ScreenBall ball in ScreenBalls)
            {
                BallChanged?.Invoke(this, new BallChangeEventArgs() { Ball = ball });
            }

        }

        //Adding single visualisation of a ball based on logicLayer.
        public void AddScreenBall(int id)
        {
            Vector2 position = logicLayer.GetPosition(id);
            float radius = logicLayer.GetRadius(id);
            ScreenBalls.Add(new ScreenBall(position.X, position.Y, radius));
        }

        //Removing single visualisation of a ball based on logicLayer.
        public void RemoveScreenBall()
        {
            ScreenBalls.Remove(ScreenBalls[ScreenBalls.Count - 1]);
        }

        public override bool ChangeSpeed(bool b, float amount)
        {
            bool success = logicLayer.ChangeSpeed(b, amount);
            if (success)
            {
                foreach (ScreenBall ball in ScreenBalls)
                {
                    BallChanged?.Invoke(this, new BallChangeEventArgs() { Ball = ball });
                }
            }

            return success;
        }
        //Triggering movement in logical layer.
        public override void MoveAllBalls()
        {
            logicLayer.UpdateArea();
        }

        public override void MoveBall(int id)
        {
            logicLayer.UpdateBall(id);
        }

        //Refreshing sphere visualization based on movement in logic layer.
        public void UpdateBall(int id)
        {
            Vector2 position = logicLayer.GetPosition(id);
            ScreenBalls[id].X = position.X;
            ScreenBalls[id].Y = position.Y;
        }
        

        #region observer

        public override void Subscribe(IObservable<int> provider)
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

        public override void OnNext(int id)
        {
            UpdateBall(id);
        }

        #endregion

        #region provider

        public override IDisposable Subscribe(IObserver<IBall> observer)
        {
            return eventObservable.Subscribe(x => observer.OnNext(x.EventArgs.Ball), ex => observer.OnError(ex), () => observer.OnCompleted());
        }

        #endregion

    }
}