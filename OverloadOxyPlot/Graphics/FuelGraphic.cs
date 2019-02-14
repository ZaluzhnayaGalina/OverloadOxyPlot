using System.Collections.Generic;
using System.Collections.ObjectModel;
using OverloadOxyPlot.Model.Interfaces;
using OxyPlot;

namespace OverloadOxyPlot.Graphics
{
    class FuelGraphic : IGraphic
    {
        public IList<DataPoint> Points { get; set; } = new ObservableCollection<DataPoint>();

        public void GetData(IReactor reactor, DayEventArgs eventArgs)
        {
            Points.Add(new DataPoint(reactor.T, eventArgs.Fuel));
        }
    }
}
