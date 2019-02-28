using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MVVMTools;
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
                OnPropertyChanged();
            }
        }
        public ReactorViewModel ReactorViewModel { get; set; }
        public ReactorViewModel StoppedReactorViewModel { get; set; }
        public ICommand ScenarioSettingsCommand { get; set; }
        public ICommand RunCommand { get; set; }
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
            var reactor = new BurningReactor();
            var stoppedReactor = new StoppedReactor();
            AssembliesList = new ObservableCollection<Assemblies>();
            ReactorViewModel = new ReactorViewModel(reactor, assemblies => AssembliesList.Add(assemblies)){ReactorName = "Работающий реактор"};
            StoppedReactorViewModel = new ReactorViewModel(stoppedReactor, assemblies => AssembliesList.Add(assemblies)) { ReactorName = "Остановленный реактор" };
            _scenarioCreator = new ScenarioCreator { Count = 2, DeltaE = 50, Days = 300, ScenarioType = ScenarioTypes.MinToMax};
            ScenarioSettingsCommand = new BaseCommand(ShowScenarioSettings);
            RunCommand = new BaseCommand(RunScenario);
            
        }

        private void RunScenario(object obj)
        {
            Cursor = Cursors.Wait;
            _scenario.Run();
            Cursor = Cursors.Arrow;

        }

        private void ShowScenarioSettings(object obj)
        {
            (new ScenarioSettings {DataContext = _scenarioCreator}).ShowDialog();
            _scenario = _scenarioCreator.CreateScenario(ReactorViewModel.Reactor, StoppedReactorViewModel.Reactor);
        }
    }  
}
