using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OverloadOxyPlot.Graphics.Interfaces;
using OverloadOxyPlot.Model;
using OxyPlot;

namespace OverloadOxyPlot.Graphics.Implementations
{
    class FunctionalGraphic: IGraphic, ISystemDataGetter
    {
        public IList<DataPoint> Points { get; set; } = new ObservableCollection<DataPoint>();
        public void GetData(SystemDayArgsEvent eventArgs)
        {
            var reactors = eventArgs.Reactors;
            double y = 0;
            foreach (var reactor in reactors)
            {
                y += reactor.Fuel;
                int j = 0;
                y += reactor.NArray.Sum(n => n * (reactor.Em - j++ * reactor.DeltaE)) * reactor.DeltaE / reactor.Em;

            }
            Points.Add(new DataPoint(eventArgs.T, y));
        }
    }
}
