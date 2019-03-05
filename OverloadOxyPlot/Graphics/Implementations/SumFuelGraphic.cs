using System.Collections.Generic;
using System.Collections.ObjectModel;
using OverloadOxyPlot.Graphics.Interfaces;
using OverloadOxyPlot.Model.Interfaces;
using OxyPlot;

namespace OverloadOxyPlot.Graphics.Implementations
{
    public class SumFuelGraphic : IGraphic, IDataGetter
    {
        private double _sumFuel;
        public IList<DataPoint> Points { get; set; } = new ObservableCollection<DataPoint>();

        public void GetData(object reactor, DayEventArgs eventArgs)
        {
            var ireactor = reactor as IReactor;
            if (ireactor is null)
                return;
            _sumFuel += eventArgs.Fuel;
            Points.Add(new DataPoint(ireactor.T, _sumFuel));
        }

        public override string ToString()
        {
            return "Суммарная подпитка свежим топливом";
        }
    }
}
