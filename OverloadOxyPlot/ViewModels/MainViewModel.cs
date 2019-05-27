using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
        public ReactorSystemViewModel ReactorSystemViewModel { get; set; }
        private ReactorSystem _reactorSystem;
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
            _reactorSystem = new ReactorSystem();
            _reactorSystem.Reactors.Add(stoppedReactor);
            _reactorSystem.Reactors.Add(reactor);
            

            _scenarioCreator = new ScenarioCreator { Count = 2, DeltaE = 50, Days = 800, ScenarioType = ScenarioTypes.Alt};
            ScenarioSettingsCommand = new BaseCommand(ShowScenarioSettings);
            BurnCommand = new BaseCommand(Burn);
            RunCommand = new BaseCommand(RunScenario);

            ReactorSystemViewModel = new ReactorSystemViewModel(_reactorSystem);
        }

        private void Burn(object obj)
        {
            _reactorSystem.DayPass();
        }

        private void RunScenario(object obj)
        {
            _scenario = _scenarioCreator.CreateScenario(_reactorSystem);
            Task.Run(() =>
            {
                _scenario.Run();
            });

        }

        private void ShowScenarioSettings(object obj)
        {
            (new ScenarioSettings {DataContext = _scenarioCreator}).ShowDialog();
            
        }
    }  
}
