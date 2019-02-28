﻿using System;
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
        private SpectrumGraphic _spectGraphic;

        public ReactorViewModel(IReactor reactor, Action<Assemblies> removeAction)
        {
            Reactor = reactor;
            _spectGraphic = new SpectrumGraphic();
            SelectedGraphic = _spectGraphic;
            var fuelGraphic = new FuelGraphic();
            reactor.DayPassed += fuelGraphic.GetData;
            _spectGraphic.GetData(Reactor);
            InsertingAssemblies = new Assemblies(2,400,500);
            RemovingAssemblies = new Assemblies(4,2000,2200);
            Graphics = new ObservableCollection<IGraphic>{_spectGraphic, fuelGraphic};
            RemoveAssembliesCommand = new BaseCommand(RemoveAssemblies, o => RemovingAssemblies!=null);
            InsertAssembliesCommand = new BaseCommand(InsertAssemblies, o=>InsertingAssemblies!=null);

            _removeAction = removeAction;

        }

        private void InsertAssemblies(object obj)
        {
            Reactor.Insert(InsertingAssemblies);
            _spectGraphic.GetData(Reactor);
        }

        private void RemoveAssemblies(object obj)
        {
            var assemblies = Reactor.Remove(RemovingAssemblies);
            _spectGraphic.GetData(Reactor);
            _removeAction(assemblies);
        }
    }
}