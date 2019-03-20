using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OverloadOxyPlot.Graphics.Interfaces;
using OverloadOxyPlot.Scenario;
using OxyPlot;

namespace OverloadOxyPlot.Graphics.Implementations
{
    class FunctionalGraphic: IGraphic, ISystemDataGetter
    {
        public IList<DataPoint> Points { get; set; } = new ObservableCollection<DataPoint>();
        public void GetData(SystemDayArgsEvents eventArgs)
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
