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
        private IGraphic _fuelandResourcefraphic;
        public ReactorSystemViewModel(ReactorSystem reactorSystem)
        {
            ReactorSystem = reactorSystem;
            ReactorViewModels = new ObservableCollection<ReactorViewModel>();
            AssembliesList = new ObservableCollection<Assemblies>();
            _fuelandResourcefraphic = new FunctionalGraphic();
            SystemGraphics.Add(_fuelandResourcefraphic);
            SelectedGraphic = _fuelandResourcefraphic;
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
