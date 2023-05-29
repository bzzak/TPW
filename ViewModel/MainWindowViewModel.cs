extern alias PresentationCore;
extern alias PresentationFramework;

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using PresentationCore::System.Windows.Media;
using Model;



namespace ViewModel
{
    public enum StatusType
    {
        Info, Error, Ok
    }
    
    public class MainWindowViewModel : ViewModelBase
    {
        // private
        private readonly ModelAPI modelLayer;
        private float speedInterval = 1.0f;
        private bool isReadyToStart = true;
        private bool isPaused = true;
        private bool isRunning = false;
        private bool isStartupExecuted = false;
        private bool canIncreaseSpeed = true;
        private bool canLowerSpeed = true;
        private bool canIncreaseSpeedInterval = true;
        private bool canLowerSpeedInterval = true;
        private bool canAddBalls = false;
        private bool canRemoveBalls = false;
        private string amountString;
        private string speedChangeStatus;
        private Brush speedChangeStatusColor;
        private Task simulationThread;
        //properties
        public CancellationToken Token { get; set; }

        public ICommand ResumeSimulationAction { get; set; }
        public ICommand StopSimulationAction { get; set; }
        public ICommand AddBallsAction { get; set; }
        public ICommand RemoveBallsAction { get; set; }
        public ICommand LowerSpeedAction { get; set; }
        public ICommand IncreaseSpeedAction { get; set; }
        public ICommand LowerSpeedIntervalAction { get; set; }
        public ICommand IncreaseSpeedIntervalAction { get; set; }

        public ObservableCollection<IBall> Balls { get; set; } = new ObservableCollection<IBall>();

        public string AmountText
        {
            get => amountString;
            set { amountString = value; RaisePropertyChanged("AmountText"); }
        }

        public float SpeedIntervalValue
        {
            get => speedInterval;
            set { speedInterval = value; RaisePropertyChanged("SpeedIntervalValue"); }
        }


        public int BallsOnScreen => Balls.Count;

        public string GetSpeedChangeStatus() => speedChangeStatus;

        public void SetSpeedChangeStatus(string info, StatusType type)
        {
            switch (type)
            {
                case StatusType.Info:
                    SpeedChangeStatusColor = new SolidColorBrush(Color.FromRgb(100, 100, 100));
                    break;
                case StatusType.Error:
                    SpeedChangeStatusColor = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    break;
                case StatusType.Ok:
                    SpeedChangeStatusColor = new SolidColorBrush(Color.FromRgb(0, 225, 0));
                    break;
                default:
                    break;
            }

            SpeedChangeStatus = info;
        }
        public string SpeedChangeStatus
        {
            get => speedChangeStatus;
            private set { speedChangeStatus = value; RaisePropertyChanged("SpeedChangeStatus"); }
        }

        public Brush SpeedChangeStatusColor
        {
            get => speedChangeStatusColor;
            set { speedChangeStatusColor = value; RaisePropertyChanged("SpeedChangeStatusColor"); }
        }

        public int BallsAmount
        {
            get
            {
                return int.TryParse(amountString, out _) && int.Parse(amountString) <= 500 && int.Parse(amountString) > 0
                    ? int.Parse(amountString)
                    : 0;
            }
        }

        
        public bool IsReadyToStart
        {
            get { return isReadyToStart; }
            set { isReadyToStart = value; RaisePropertyChanged("IsReadyToStart"); }
        }

        
        public bool CanStop
        {
            get => isRunning;
            set { isRunning = value; RaisePropertyChanged("CanStop"); }
        }
        public bool CanResume
        {
            get => isPaused;
            set { isPaused = value; RaisePropertyChanged("CanResume"); }
        }
       
        public bool CanIncreaseSpeed
        {
            get => canIncreaseSpeed;
            set { canIncreaseSpeed = value; RaisePropertyChanged("CanIncreaseSpeed"); }
        }
        public bool CanLowerSpeed
        {
            get => canLowerSpeed;
            set { canLowerSpeed = value; RaisePropertyChanged("CanLowerSpeed"); }
        }

        public bool CanIncreaseSpeedInterval
        {
            get => canIncreaseSpeedInterval;
            set { canIncreaseSpeedInterval = value; RaisePropertyChanged("CanIncreaseSpeedInterval"); }
        }
        public bool CanLowerSpeedInterval
        {
            get => canLowerSpeedInterval;
            set { canLowerSpeedInterval = value; RaisePropertyChanged("CanLowerSpeedInterval"); }
        }
        public bool CanAddBalls
        {
            get => canAddBalls;
            set { canAddBalls = value; RaisePropertyChanged("CanAddBalls"); }
        }
        public bool CanRemoveBalls
        {
            get => canRemoveBalls;
            set { canRemoveBalls = value; RaisePropertyChanged("CanRemoveBalls"); }
        }
        // constructors
        public MainWindowViewModel()
        {
           
            ResumeSimulationAction = new RelayCommand(() => ResumeSimulation());
            StopSimulationAction = new RelayCommand(() => StopSimulation());
            AddBallsAction = new RelayCommand(() => AddBalls());
            RemoveBallsAction = new RelayCommand(() => RemoveBalls());
            IncreaseSpeedAction = new RelayCommand(() => IncreaseSpeed());
            LowerSpeedAction = new RelayCommand(() => LowerSpeed());
            IncreaseSpeedIntervalAction = new RelayCommand(() => IncreaseSpeedInterval());
            LowerSpeedIntervalAction = new RelayCommand(() => LowerSpeedInterval());

            SetSpeedChangeStatus("WAITING FOR ACTION", StatusType.Info);

            modelLayer = ModelAPI.CreateModelLayer();
            IDisposable observer = modelLayer.Subscribe<IBall>(x => Balls.Add(x));
            for (int i = 0; i < modelLayer.ScreenBalls.Count; i++)
            {
                Balls.Add(modelLayer.ScreenBalls[i]);
            }
            RaisePropertyChanged("BallsOnScreen");
        }


        // methods
        private void StartupRoutine()
        {
            if (BallsAmount > 0)
            {
                IsReadyToStart = false;

                modelLayer.AddBalls(BallsAmount);
                
                Balls.Clear();
                for (int i = 0; i < modelLayer.ScreenBalls.Count; i++)
                {
                    Balls.Add(modelLayer.ScreenBalls[i]);
                }
                RaisePropertyChanged("BallsOnScreen");

                CanAddBalls = true;
                CanRemoveBalls = true;
                isStartupExecuted = true;
            }
        }
        private void ResumeSimulation()
        {
            if (IsReadyToStart) StartupRoutine();
            if (isStartupExecuted)
            {
                CanStop = true;
                CanResume = false;

                modelLayer.MoveAllBalls();
                //simulationThread = new Task(Simulation, Token);
                //simulationThread.Start();
            }
        }
        private void StopSimulation()
        {
            
        }

        private void Simulation()
        {
            while (CanStop)
            {
                lock (modelLayer.ScreenBalls)
                {
                    Balls = new ObservableCollection<IBall>(modelLayer.ScreenBalls);
                }
                RaisePropertyChanged(nameof(Balls));
            }
        }
        private void AddBalls()
        {
            int startIndex = BallsOnScreen;
            int endIndex = BallsOnScreen + BallsAmount - 1;
            modelLayer.AddBalls(BallsAmount);
            RaisePropertyChanged("BallsOnScreen");
            for(int i = startIndex; i < endIndex; i++)
            {
                modelLayer.MoveBall(i);
            }
        }
        private void RemoveBalls()
        {
            modelLayer.RemoveBalls(BallsAmount);
            RaisePropertyChanged("BallsOnScreen");
        }
        private void IncreaseSpeed()
        {
            bool success = modelLayer.ChangeSpeed(true, SpeedIntervalValue);
            SetSpeedInfo(success, true);
        }
        private void LowerSpeed()
        {
            bool success = modelLayer.ChangeSpeed(false, SpeedIntervalValue);
            SetSpeedInfo(success, false);
        }

        private void SetSpeedInfo(bool success, bool sign)
        {
            if (success && sign) SetSpeedChangeStatus("SPEED INCREASED", StatusType.Ok);
            else if (success && !sign) SetSpeedChangeStatus("SPEED DECREASED", StatusType.Ok);
            else if (!success && sign) SetSpeedChangeStatus("SPEED EXCEEDS MAX VALUE", StatusType.Error);
            else if (!success && !sign) SetSpeedChangeStatus("SPEED EXCEEDS MIN VALUE", StatusType.Error);

        }
        private void IncreaseSpeedInterval()
        {
            SpeedIntervalValue += 0.5f;

            if (SpeedIntervalValue + 0.5 > 5)
            {
                CanIncreaseSpeedInterval = false;
            }
            CanLowerSpeedInterval = true;
        }

        private void LowerSpeedInterval()
        {
            SpeedIntervalValue -= 0.5f;

            if (SpeedIntervalValue - 0.5 <= 0)
            {
                CanLowerSpeedInterval = false;
            }
            CanIncreaseSpeedInterval = true;
        }

    }
}
