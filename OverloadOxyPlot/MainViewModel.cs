using System;
using OverloadOxyPlot.Model;
using System.Windows.Input;
using System.Windows.Forms;
using MVVMTools;
using OxyPlot;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OxyPlot.Wpf;
using OverloadOxyPlot.Scenario;

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

        public ObservableCollection<IScenario> Scenarios { get; set; }
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
        private ICommand _savePlotCommand;
        private string _progressBarVisibility;
        private double _progressBarValue=20;
        private System.Windows.Input.Cursor _cursor;
        private IScenario _scenario;
        public IScenario Scenario
        {
            get => _scenario;
            set
            {
                _scenario = value;
            }
        }
        public IReactor Reactor { get; set; }
        public IReactor StoppedReactor { get; set; }
        public ICommand StoppedReactorRemoveCommand => _stoppedReactorRemoveCommand ?? (_stoppedReactorRemoveCommand = new BaseCommand(StoppedReactorRemove));
        public ICommand StoppedReactorInsertCommand => _stoppedReactorInsertCommand ?? (_stoppedReactorInsertCommand = new BaseCommand(StoppedReactorInsert, InsertPossible));
        public ICommand ScenarioSettingsCommand => _scenarioSettingsCommand ?? (_scenarioSettingsCommand = new BaseCommand(ScenarioSet));
        public ICommand RunCommand => _runCommand ?? (_runCommand = new BaseCommand(RunScenario));
        public ICommand SavePlotCommand=>_savePlotCommand ?? (_savePlotCommand = new BaseCommand(SavePlot));

        private void SavePlot(object obj)
        {
            var model = obj as PlotModel;
            if (model is null)
                return;
            var fd = new SaveFileDialog();
            fd.ShowDialog();
            if (fd.FileName is null || String.IsNullOrEmpty(fd.FileName))
                return;
            var pngExporter = new PngExporter { Width = 600, Height = 400, Background = OxyColors.White };
            pngExporter.ExportToFile(model,fd.FileName);
        }

        public System.Windows.Input.Cursor Cursor
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


        private void RunScenario(object obj)
        {
            _scenario.Run();            
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
            AssembliesList.Remove(Assemblies);;
        }

        private void StoppedReactorRemove(object obj)
        {
            AssembliesList.Add(StoppedReactor.Remove(StoppedReactorAssemblies));
        }

        public ICommand RemoveCommand => _removeCommand ?? (_removeCommand = new BaseCommand(Remove));

        private void Remove(object obj)
        {
            AssembliesList.Add(Reactor.Remove(BurningReactorAssemblies));
        }

        public ICommand InsertCommand => _insertCommand ?? (_insertCommand = new BaseCommand(Insert, InsertPossible));

        private void Insert(object obj)
        {
            Reactor.Insert(Assemblies);
            AssembliesList.Remove(Assemblies);
        }

        
        public ICommand BurnCommand => _burnCommand ?? (_burnCommand = new BaseCommand(Burn));

        private void Burn(object obj)
        {
            for (int i = 0; i < 1.0 / Reactor.DeltaT; i++)
            {
                Reactor.Burn();
                StoppedReactor.Burn();
            }
        }

         public MainViewModel()
        {
            Reactor = new BurningReactor();
            ((BurningReactor)Reactor).FuelEvent += OnFuelEvent;
            StoppedReactor = new StoppedReactor();
           
            AssembliesList = new ObservableCollection<Assemblies>();
            StoppedReactorAssemblies = new Assemblies(1.0, 400.0,500.0);
            BurningReactorAssemblies = new Assemblies(2.0, 2000.0, 2500.0);
            Assemblies = new Assemblies();

            Scenarios = new ObservableCollection<IScenario>();
            Scenarios.Add(new ScenarioMinToMax(Reactor, StoppedReactor));
            Scenarios.Add(new ScenarioAlt(Reactor, StoppedReactor));
        }

        private void OnFuelEvent(Assemblies fuelling)
        {
            _fuel += fuelling.Count;
        }
    }  
}
