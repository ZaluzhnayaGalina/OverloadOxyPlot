using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using OverloadOxyPlot.Graphics.Interfaces;
using OverloadOxyPlot.Model;
using OverloadOxyPlot.Model.Interfaces;
using OxyPlot;

namespace OverloadOxyPlot.Graphics.Implementations
{
    class FunctionalGraphic: IGraphic, ISystemDataGetter
    {
        public IList<DataPoint> Points { get; set; } = new ObservableCollection<DataPoint>();
        private double _sumFuel = 0.0;
        public void GetData(SystemDayArgsEvent eventArgs)
        {
            var reactors = eventArgs.Reactors;
            double y = 0;
            double r = 0;
            foreach (var reactor in reactors)
            {
                _sumFuel += reactor.Fuel;
                r+= GetUnusedResources(reactor);
            }
            y = r + _sumFuel;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Points.Add(new DataPoint(eventArgs.T, y));
                if (eventArgs.T % 80 == 0)
                {
                    using (var stream = new StreamWriter("data2.txt", true))
                    {
                        stream.WriteLine(eventArgs.T + "\t" + y);
                    }
                }
            });
        }
        private double GetUnusedResources(IReactor reactor)
        {
            double sum = 0;
            for (int i = 0; i < reactor.NArray.Length; i++)
            {
                if (i * reactor.DeltaE > reactor.Em)
                    break;
                sum += (reactor.Em - i * reactor.DeltaE) * reactor.NArray[i];
            }
            return sum * reactor.DeltaE / reactor.Em;
        }
    }
}
