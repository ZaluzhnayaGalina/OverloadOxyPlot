using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OverloadOxyPlot.Model.Interfaces;
using OxyPlot;

namespace OverloadOxyPlot.Graphics
{
    class CustomGraphic : IGraphic
    {
        public IList<DataPoint> Points { get; set; }
        private Func<DayEventArgs, double> _function;
        public CustomGraphic(Func<DayEventArgs,double> function)
        {
            _function = function;
            Points = new ObservableCollection<DataPoint>();
        }
        public void GetData(IReactor reactor, DayEventArgs eventArgs)
        {
            Points.Add(new DataPoint(reactor.T, _function(eventArgs)));
        }
    }
}
