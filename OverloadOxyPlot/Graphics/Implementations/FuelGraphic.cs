using System.Collections.Generic;
using System.Collections.ObjectModel;
using OverloadOxyPlot.Graphics.Interfaces;
using OverloadOxyPlot.Model.Interfaces;
using OxyPlot;

namespace OverloadOxyPlot.Graphics.Implementations
{
    class FuelGraphic : IGraphic, IDataGetter
    {
        public IList<DataPoint> Points { get; set; } = new ObservableCollection<DataPoint>();

        public void GetData(object reactor, DayEventArgs eventArgs)
        {
            var ireactor = reactor as IReactor;
            if (ireactor is null)
                return;
            Points.Add(new DataPoint(ireactor.T, eventArgs.Fuel));
        }
    }
}
