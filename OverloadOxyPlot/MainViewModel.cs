using System;
using OverloadOxyPlot.Model;
using System.Windows.Input;
using MVVMTools;
using OxyPlot;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace OverloadOxyPlot
{
    internal class MainViewModel : BaseNotifyPropertyChanged
    {
        private IList<DataPoint> _points;
        public IList<Assemblies> AssembliesList { get; set; }
        private Assemblies _assemblies;
        public Assemblies Assemblies
        { get
            {
                return _assemblies;
            }
            set
            {
                if (value==_assemblies)
                    return;
                _assemblies = value;
                ((BaseCommand)InsertCommand).RaiseCanExcuteChanged();
                ((BaseCommand)StoppedReactorInsertCommand).RaiseCanExcuteChanged();
                OnPropertyChanged();
            }
        }
        private int _day = 1;
        private double _fuel = 0;
        public IList<DataPoint> FuellingPoints { get; set; }// 
        public IList<DataPoint> ConstFuellingPoints { get; set; }// 
        public Assemblies StoppedReactorAssemblies { get; set; }
        public Assemblies BurningReactorAssemblies { get; set; }
        private IList<DataPoint> _points2;
        private ICommand _burnCommand;
        private ICommand _removeCommand;
        private ICommand _insertCommand;
        private ICommand _stoppedReactorRemoveCommand;
        private ICommand _stoppedReactorInsertCommand;
        private ICommand _scenarioSettingsCommand;
        private ICommand _runCommand;
        private string _progressBarVisibility;
        private double _progressBarValue=20;
        private Cursor _cursor;
        private Scenario _scenario = new Scenario();
        public IReactor Reactor { get; set; }
        public IReactor StoppedReactor { get; set; }
        public ICommand StoppedReactorRemoveCommand => _stoppedReactorRemoveCommand ?? (_stoppedReactorRemoveCommand = new BaseCommand(StoppedReactorRemove));
        public ICommand StoppedReactorInsertCommand => _stoppedReactorInsertCommand ?? (_stoppedReactorInsertCommand = new BaseCommand(StoppedReactorInsert, InsertPossible));
        public ICommand ScenarioSettingsCommand => _scenarioSettingsCommand ?? (_scenarioSettingsCommand = new BaseCommand(ScenarioSet));
        public ICommand RunCommand => _runCommand ?? (_runCommand = new BaseCommand(RunScenario));
        public Cursor Cursor
        {
            get=>_cursor;
            set
            {
                if (value == _cursor)
                    return;
                _cursor = value;
                OnPropertyChanged();
            }
        }
        public string ProgressBarVisibility
        {
            get => _progressBarVisibility;
            set
            {
                if (value == _progressBarVisibility)
                    return;
                _progressBarVisibility = value;
                OnPropertyChanged();
            }
        }
        public double ProgressBarValue
        {
            get=>_progressBarValue;
            set
            {
                if (value == _progressBarValue)
                    return;
                _progressBarValue = value;
                OnPropertyChanged();
            }
        }
        private void RunScenario(object obj)
        {
            ProgressBarVisibility = "Visible";
            Cursor = Cursors.Wait;
            if (_scenario.SelectedWay == 0)
                RunMinToMax();
            if (_scenario.SelectedWay == 1)
                RunMaxToMin();
            if (_scenario.SelectedWay == 2)
                RunAlt();
            //switch (_scenario.SelectedWay)
            //{
            //    case 1:
            //        RumMaxToMin();
            //        break;
            //    case 2:
            //        RunAlt();
            //        break;
            //    default:
            //        RunMinToMax();
            //        break;
            //}
            ProgressBarVisibility = "Collapsed";
            ProgressBarValue = 0;
            Cursor = Cursors.Arrow;
        }

        private void RunMinToMax()
        {
            Assemblies a = new Assemblies();
            for (int i=0; i<_scenario.Days; i++)
            {
                a.Count = _scenario.Count;
                a.E1 = StoppedReactor.NArray.FindIndex(x => x > 0) * StoppedReactor.DeltaE;
                a.E2 = a.E1 + _scenario.DeltaE;
                var a1 = StoppedReactor.Remove(a);
                Reactor.Insert(a1);
                for (int j = 0; j < 1.0 / Reactor.DeltaT; j++)
                {
                    Reactor.Burn();
                    StoppedReactor.Burn();
                }
                FuellingPoints.Add(new DataPoint(_day, _fuel));
                ConstFuellingPoints.Add(new DataPoint(_day, Reactor.Q0));
                _day++;
                _fuel = 0;
                //ProgressBarValue = (double)(i + 1) / (double)_scenario.Days * 100;
            }
            GetReactorPoints(Reactor, Points);
            GetReactorPoints(StoppedReactor, Points2);
        }

        private void RunAlt()
        {
            Assemblies a = new Assemblies();
            Assemblies a1 = new Assemblies();
            bool tmp=true;
            for (int i = 0; i < _scenario.Days; i++)
            {
                a.Count = _scenario.Count;
                if (tmp)
                {
                    a.E1 = StoppedReactor.NArray.FindIndex(x => x > 0) * StoppedReactor.DeltaE;
                    a.E2 = a.E1 + _scenario.DeltaE;
                }
                else
                {
                    for (int k = StoppedReactor.NArray.Count - 1; k >= 0; k--)
                    {
                        if (Math.Abs(StoppedReactor.NArray[k]) > 0.01)
                        {
                            a.E2 = k * StoppedReactor.DeltaE;
                            break;
                        }
                    }
                    a.E1 = a.E2 - _scenario.DeltaE;
                }
                a1 = StoppedReactor.Remove(a);

                Reactor.Insert(a1);
                for (int j = 0; j < 1.0 / Reactor.DeltaT; j++)
                {
                    Reactor.Burn();
                    StoppedReactor.Burn();
                }
                FuellingPoints.Add(new DataPoint(_day, _fuel));
                ConstFuellingPoints.Add(new DataPoint(_day, Reactor.Q0));
                _day++;
                _fuel = 0;
                tmp = !tmp;
                //ProgressBarValue = (double)(i + 1) / (double)_scenario.Days * 100;
            }
            GetReactorPoints(Reactor, Points);
            GetReactorPoints(StoppedReactor, Points2);
        }

        private void RunMaxToMin()
        {
            
            Assemblies a = new Assemblies();
            for (int i = 0; i < _scenario.Days; i++)
            {
                a.Count = _scenario.Count;
                for(int k= StoppedReactor.NArray.Count-1; k>=0;k--)
                {
                    if (Math.Abs(StoppedReactor.NArray[k])>0.01)
                    {
                        a.E2 = k * StoppedReactor.DeltaE;
                        break;
                    }
                }
                a.E1 = a.E2 - _scenario.DeltaE;
                var a1 = StoppedReactor.Remove(a);
                Reactor.Insert(a1);
                for (int j = 0; j < 1.0 / Reactor.DeltaT; j++)
                {
                    Reactor.Burn();
                    StoppedReactor.Burn();
                }
                FuellingPoints.Add(new DataPoint(_day, _fuel));
                ConstFuellingPoints.Add(new DataPoint(_day, Reactor.Q0));
                _day++;
                _fuel = 0;
              //  ProgressBarValue = ProgressBarValue + 1.0 / (double)_scenario.Days * 100;
            }
            GetReactorPoints(Reactor, Points);
            GetReactorPoints(StoppedReactor, Points2);
        }

        private void ScenarioSet(object obj)
        {
            var view = new ScenarioSettings { DataContext = _scenario};
            view.ShowDialog();
        }

        private bool InsertPossible(object obj)
        {
            return Assemblies != null;
        }

        private void StoppedReactorInsert(object obj)
        {
            StoppedReactor.Insert(Assemblies);
            AssembliesList.Remove(Assemblies);
            GetReactorPoints(StoppedReactor, Points2);
        }

        private void StoppedReactorRemove(object obj)
        {
            AssembliesList.Add(StoppedReactor.Remove(StoppedReactorAssemblies));
            GetReactorPoints(StoppedReactor, Points2);
        }

        public ICommand RemoveCommand => _removeCommand ?? (_removeCommand = new BaseCommand(Remove));

        private void Remove(object obj)
        {
            AssembliesList.Add(Reactor.Remove(BurningReactorAssemblies));
            GetReactorPoints(Reactor,Points);
        }

        public ICommand InsertCommand => _insertCommand ?? (_insertCommand = new BaseCommand(Insert, InsertPossible));

        private void Insert(object obj)
        {
            Reactor.Insert(Assemblies);
            AssembliesList.Remove(Assemblies);
            GetReactorPoints(Reactor, Points);
        }

        public IList<DataPoint> Points
        {
            get=>_points;
            set
            {
                if (value.Equals(_points))
                    return;
                _points = value;
                OnPropertyChanged();
            }
        }
        public IList<DataPoint> Points2
        {
            get => _points2;
            set
            {
                if (value.Equals(_points2))
                    return;
                _points2 = value;
                OnPropertyChanged();
            }
        }
        public ICommand BurnCommand => _burnCommand ?? (_burnCommand = new BaseCommand(Burn));

        private void Burn(object obj)
        {
            for (int i = 0; i < 1.0 / Reactor.DeltaT; i++)
            {
                Reactor.Burn();
                StoppedReactor.Burn();
            }
            //((BurningReactor)Reactor).Fuel();
            GetReactorPoints(Reactor,Points);
            GetReactorPoints(StoppedReactor,Points2);
            FuellingPoints.Add(new DataPoint(_day, _fuel));
            ConstFuellingPoints.Add(new DataPoint(_day, Reactor.Q0));
            _day++;
            _fuel = 0;
        }

        private void GetReactorPoints(IContainer container, IList<DataPoint> points)
        {
            points.Clear();
            for (int j = 0; j < container.NArray.Count; j++)
                points.Add(new DataPoint(j * container.DeltaE, container.NArray[j]));

        }

         public MainViewModel()
        {
            Reactor = new BurningReactor();
            ((BurningReactor)Reactor).FuelEvent += OnFuelEvent;
            StoppedReactor = new StoppedReactor();
            Points = new ObservableCollection<DataPoint>();
            Points2 = new ObservableCollection<DataPoint>();
            GetReactorPoints(Reactor, Points);
            GetReactorPoints(StoppedReactor, Points2);
            AssembliesList = new ObservableCollection<Assemblies>();
            StoppedReactorAssemblies = new Assemblies(1.0, 400.0,500.0);
            BurningReactorAssemblies = new Assemblies(2.0, 2000.0, 2500.0);
            Assemblies = new Assemblies();
            FuellingPoints = new ObservableCollection<DataPoint>();
            ConstFuellingPoints = new ObservableCollection<DataPoint>();
        }

        private void OnFuelEvent(Assemblies fuelling)
        {
            _fuel += fuelling.Count;
        }
    }  
}
