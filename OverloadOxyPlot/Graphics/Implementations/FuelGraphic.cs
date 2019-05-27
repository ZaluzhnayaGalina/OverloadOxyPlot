using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using OverloadOxyPlot.Graphics.Interfaces;
using OverloadOxyPlot.Model.Interfaces;
using OxyPlot;

namespace OverloadOxyPlot.Graphics.Implementations
{
    public class FuelGraphic : IGraphic, IDataGetter
    {
        public IList<DataPoint> Points { get; set; } = new ObservableCollection<DataPoint>();

        public void GetData(object reactor, ReactorDayEventArgs eventArgs)
        {
            var ireactor = reactor as IReactor;
            if (ireactor is null)
                return;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Points.Add(new DataPoint(ireactor.T, ireactor.Fuel));
                if (ireactor.T % 10 == 0)
                {
                    using (var stream = new StreamWriter("fuel.txt", true))
                    {
                        stream.WriteLine(ireactor.T + "\t" + ireactor.Fuel);
                    }
                }
            });
        }

        public override string ToString()
        {
            return "Подпитка свежим топливом в зависимости от времени";
        }
    }
}
