using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MVVMTools;
using OverloadOxyPlot.Graphics.Implementations;
using OverloadOxyPlot.Model;
using OverloadOxyPlot.Model.Implementations;
using OverloadOxyPlot.Scenario;
using OverloadOxyPlot.Views;

namespace OverloadOxyPlot.ViewModels
{
    internal class MainViewModel : BaseNotifyPropertyChanged
    {
        public IList<Assemblies> AssembliesList { get; set; }
        private Assemblies _assemblies;
        public Assemblies Assemblies
        {
            get => _assemblies;
            set
            {
                if (value==_assemblies)
                    return;
                _assemblies = value;
                ReactorViewModel.InsertingAssemblies = _assemblies;
                StoppedReactorViewModel.InsertingAssemblies = _assemblies;
                OnPropertyChanged();
            }
        }
        public ReactorViewModel ReactorViewModel { get; set; }
        public ReactorViewModel StoppedReactorViewModel { get; set; }
        public ICommand ScenarioSettingsCommand { get; set; }
        public ICommand RunCommand { get; set; }
        public  ICommand BurnCommand { get; set; }
        private IScenario _scenario;
        private ScenarioCreator _scenarioCreator;
        private Cursor _cursor;

        public Cursor Cursor
        {
            get => _cursor;
            set
            {
                _cursor = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            var reactor = new Reactor();
            reactor.BurnBehavior = new BurnBehavior(reactor);
            var stoppedReactor = new Reactor();
            stoppedReactor.BurnBehavior = new NonBurnBehavior(stoppedReactor);
            AssembliesList = new ObservableCollection<Assemblies>();
            ReactorViewModel = new ReactorViewModel(reactor, assemblies =>  AssembliesList.Add(assemblies), assemblies=>AssembliesList.Remove(Assemblies)){ReactorName = "Работающий реактор"};
            ReactorViewModel.AddGraphic(new FuelGraphic());
            ReactorViewModel.AddGraphic(new SumFuelGraphic());
            StoppedReactorViewModel = new ReactorViewModel(stoppedReactor, assemblies => AssembliesList.Add(assemblies), assemblies => AssembliesList.Remove(Assemblies)) { ReactorName = "Остановленный реактор"};
            _scenarioCreator = new ScenarioCreator { Count = 2, DeltaE = 50, Days = 300, ScenarioType = ScenarioTypes.MinToMax};
            ScenarioSettingsCommand = new BaseCommand(ShowScenarioSettings);
            BurnCommand = new BaseCommand(Burn);
            RunCommand = new BaseCommand(RunScenario);
            
        }

        private void Burn(object obj)
        {
            ReactorViewModel.Reactor.DayPass();
            StoppedReactorViewModel.Reactor.DayPass();

        }

        private void RunScenario(object obj)
        {
            _scenario = _scenarioCreator.CreateScenario(ReactorViewModel.Reactor, StoppedReactorViewModel.Reactor);
            Cursor = Cursors.Wait;
            _scenario.Run();
            Cursor = Cursors.Arrow;

        }

        private void ShowScenarioSettings(object obj)
        {
            (new ScenarioSettings {DataContext = _scenarioCreator}).ShowDialog();
            
        }
    }  
}
