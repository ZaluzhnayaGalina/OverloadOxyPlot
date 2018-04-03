using System;
using OverloadOxyPlot.Model;
using System.Windows.Input;
using MVVMTools;
using OxyPlot;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OverloadOxyPlot
{
    internal class MainViewModel : BaseNotifyPropertyChanged
    {
        private IList<DataPoint> _points;
        public IList<Assemblies> AssembliesList { get; set; }
        public Assemblies Assemblies { get; set; }
        public Assemblies AssembliesToRemove { get; set; }
        private IList<DataPoint> _points2;
        private ICommand _burnCommand;
        private ICommand _removeCommand;
        private ICommand _insertCommand;
        public IReactor Reactor { get; set; }
        public IReactor StoppedReactor { get; set; }

        public ICommand RemoveCommand => _removeCommand ?? (_removeCommand = new BaseCommand(Remove));

        private void Remove(object obj)
        {
            AssembliesList.Add(StoppedReactor.Remove(AssembliesToRemove));

            GetReactorPoints(StoppedReactor,Points2);
        }

        public ICommand InsertCommand => _insertCommand ?? (_insertCommand = new BaseCommand(Insert));

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
            GetReactorPoints(Reactor,Points);
            GetReactorPoints(StoppedReactor,Points2);
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
            StoppedReactor = new StoppedReactor();
            Points = new ObservableCollection<DataPoint>();
            Points2 = new ObservableCollection<DataPoint>();
            GetReactorPoints(Reactor, Points);
            GetReactorPoints(StoppedReactor, Points2);
            AssembliesList = new ObservableCollection<Assemblies>();
            AssembliesToRemove = new Assemblies();
            Assemblies = new Assemblies();
        }
    }

  
}
