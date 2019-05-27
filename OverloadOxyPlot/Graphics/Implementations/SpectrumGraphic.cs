using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using OverloadOxyPlot.Graphics.Interfaces;
using OverloadOxyPlot.Model.Interfaces;
using OxyPlot;

namespace OverloadOxyPlot.Graphics.Implementations
{
    public class SpectrumGraphic: IGraphic, IDataGetter
    { 
        public IList<DataPoint> Points { get; set; } = new ObservableCollection<DataPoint>();

        public void GetData(object reactor, ReactorDayEventArgs eventArgs)
        {
            var ireactor = reactor as IReactor;
            if (ireactor is null)
                return;
            Application.Current.Dispatcher.Invoke(() =>
            {
                Points.Clear();
                for (int j = 0; j < ireactor.NArray.Length; j++)
                    Points.Add(new DataPoint(j * ireactor.DeltaE, ireactor.NArray[j]));
            });
        }

        public override string ToString()
        {
            return "Спектр ТВС";
        }
    }
}
