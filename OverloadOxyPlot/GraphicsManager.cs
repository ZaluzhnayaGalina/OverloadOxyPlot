using OverloadOxyPlot.Annotations;
using OverloadOxyPlot.Model;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OverloadOxyPlot
{
    class GraphicsManager: INotifyPropertyChanged
    {
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private IList<DataPoint> _points2;
        private IList<DataPoint> _points;

        public event PropertyChangedEventHandler PropertyChanged;

        public IList<DataPoint> Points
        {
            get => _points;
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
        public IList<DataPoint> UnusedResource1 { get; set; }
        public IList<DataPoint> UnusedResource2 { get; set; }
        public IList<DataPoint> TotalUnusedResource { get; set; }
        private double GetUnusedResources(IReactor reactor)
        {
            double sum = 0;
            for (int i = 0; i < reactor.NArray.Count; i++)
            {
                if (i * reactor.DeltaE > reactor.Em)
                    break;
                sum += (reactor.Em - i * reactor.DeltaE) * reactor.NArray[i];
            }
            return sum * reactor.DeltaE / reactor.Em;
        }

        private void GetReactorPoints(Model.IContainer container, IList<DataPoint> points)
        {
            points.Clear();
            for (int j = 0; j < container.NArray.Count; j++)
                points.Add(new DataPoint(j * container.DeltaE, container.NArray[j]));

        }
    }
}
