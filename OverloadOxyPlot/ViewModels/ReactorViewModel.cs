using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MVVMTools;
using OverloadOxyPlot.Graphics.Implementations;
using OverloadOxyPlot.Graphics.Interfaces;
using OverloadOxyPlot.Model;
using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.ViewModels
{
    public class ReactorViewModel
    {
        public string ReactorName { get; set; }
        public IReactor Reactor { get; private set; }
        public ObservableCollection<IGraphic> Graphics { get; set; }
        public ICommand RemoveAssembliesCommand { get; set; }
        public ICommand InsertAssembliesCommand { get; set; }
        public IGraphic SelectedGraphic { get; set; }
        public Assemblies InsertingAssemblies { get; set; }
        public Assemblies RemovingAssemblies { get; set; }
        private Action<Assemblies> _removeAction;
        private Action<Assemblies> _insertAction;
        private SpectrumGraphic _spectGraphic;

        public ReactorViewModel(IReactor reactor, Action<Assemblies> removeAction, Action<Assemblies> insertAction)
        {
            Reactor = reactor;
            _spectGraphic = new SpectrumGraphic();
            _spectGraphic.GetData(Reactor, null);
            SelectedGraphic = _spectGraphic;
            var fuelGraphic = new FuelGraphic();
            reactor.DayPassed += fuelGraphic.GetData;
            reactor.DayPassed += _spectGraphic.GetData;
            InsertingAssemblies = new Assemblies(2,400,500);
            RemovingAssemblies = new Assemblies(4,2000,2200);
            Graphics = new ObservableCollection<IGraphic>{_spectGraphic, fuelGraphic};
            RemoveAssembliesCommand = new BaseCommand(RemoveAssemblies, o => RemovingAssemblies!=null);
            InsertAssembliesCommand = new BaseCommand(InsertAssemblies, o=>InsertingAssemblies!=null);

            _removeAction = removeAction;
            _insertAction = insertAction;

        }

        private void InsertAssemblies(object obj)
        {
            Reactor.Insert(InsertingAssemblies);
            _insertAction(InsertingAssemblies);
            _spectGraphic.GetData(Reactor, null);
        }

        private void RemoveAssemblies(object obj)
        {
            var assemblies = Reactor.Remove(RemovingAssemblies);
            _removeAction(assemblies);
            _spectGraphic.GetData(Reactor, null);
        }
    }
}
