using MVVMTools;
using OverloadOxyPlot.Graphics.Implementations;
using OverloadOxyPlot.Graphics.Interfaces;
using OverloadOxyPlot.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OverloadOxyPlot.ViewModels
{
    public class ReactorSystemViewModel : BaseNotifyPropertyChanged
    {
        public IList<ReactorViewModel> ReactorViewModels { get; set; }
        public ReactorSystem ReactorSystem { get; }
        private FunctionalGraphic _fuelAndResourceGraphic;
        public ReactorSystemViewModel(ReactorSystem reactorSystem)
        {
            ReactorSystem = reactorSystem;
            ReactorViewModels = new ObservableCollection<ReactorViewModel>();
            int i = 1;
            AssembliesList = new ObservableCollection<Assemblies>();
            foreach (var reactor in ReactorSystem.Reactors)
            {
                var reactorViewModel = new ReactorViewModel(reactor,
                    assemblies => AssembliesList.Add(assemblies), assemblies => AssembliesList.Remove(Assemblies));
                reactorViewModel.ReactorName = "Реактор " + i.ToString("D");
                reactorViewModel.AddGraphic(new FuelGraphic());
                reactorViewModel.AddGraphic(new SumFuelGraphic());
                i++;
                ReactorViewModels.Add(reactorViewModel);
            }
            
            _fuelAndResourceGraphic = new FunctionalGraphic();
            ReactorSystem.DayPassed += _fuelAndResourceGraphic.GetData;
            SystemGraphics.Add(_fuelAndResourceGraphic);
            SelectedGraphic = _fuelAndResourceGraphic;
        }
        public IList<Assemblies> AssembliesList { get; set; }
        private Assemblies _assemblies;
        public Assemblies Assemblies
        {
            get => _assemblies;
            set
            {
                if (value == _assemblies)
                    return;
                _assemblies = value;
                foreach (var reactor in ReactorViewModels)
                {
                    reactor.InsertingAssemblies = _assemblies;
                }
                OnPropertyChanged();
            }
        }
        public ObservableCollection<IGraphic> SystemGraphics { get; set; } = new ObservableCollection<IGraphic>();
        public IGraphic SelectedGraphic { get; set; }
    }
}
