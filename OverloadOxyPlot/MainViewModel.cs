using OverloadOxyPlot.Model;
using System.Windows.Input;
using System.Windows;
using MVVMTools;
using System;
using System.Diagnostics;
using OxyPlot;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.Linq;

namespace OverloadOxyPlot
{
    internal class MainViewModel: BaseNotifyPropertyChanged
    {
        public ReactorSystem System { get; set; }
        private Cursor _cursor;
        private double _t;
        public PlotModel PlotModelQ { get; set; }
        public PlotModel PlotModelN { get; set; }
        public double T {
            get =>_t;
            set
            {

                if (_t == value || value==System.Tmax)
                    return;
                _t = value;
                int n = (int)Math.Ceiling(_t / System.DeltaT);
                if (n >= System.Reactor1.QArray.Count)
                    return;
                var Reactor1PointsQ = new ObservableCollection<DataPoint>();
                var Reactor2PointsQ = new ObservableCollection<DataPoint>();
                var Reactor1PointsN = new ObservableCollection<DataPoint>();
                var Reactor2PointsN = new ObservableCollection<DataPoint>();
                for (int i=0; i<System.Reactor1.QArray[0].Count; i++)
                {
                    Reactor1PointsQ.Add(new DataPoint(i * System.DeltaE, System.Reactor1.QArray[n][i]));
                    Reactor2PointsQ.Add(new DataPoint(i * System.DeltaE, System.Reactor2.QArray[n][i]));

                    Reactor1PointsN.Add(new DataPoint(i * System.DeltaE, System.Reactor1.NArray[n][i]));
                    Reactor2PointsN.Add(new DataPoint(i * System.DeltaE, System.Reactor2.NArray[n][i]));
                }
                
                ((LineSeries)PlotModelQ.Series[0]).Points.Clear();
                ((LineSeries)PlotModelQ.Series[0]).Points.AddRange(Reactor1PointsQ);
                PlotModelQ.Axes[0].Maximum = Math.Max(Reactor1PointsQ.Max(x => x.X),Reactor2PointsQ.Max(x=>x.X)) * 1.1;
                PlotModelQ.Axes[1].Maximum = Math.Max(Reactor1PointsQ.Max(x => x.Y), Reactor2PointsQ.Max(x => x.Y)) * 1.1;
                ((LineSeries)PlotModelQ.Series[1]).Points.Clear();
                ((LineSeries)PlotModelQ.Series[1]).Points.AddRange(Reactor2PointsQ);

                ((LineSeries)PlotModelN.Series[0]).Points.Clear();
                ((LineSeries)PlotModelN.Series[0]).Points.AddRange(Reactor1PointsN);
                PlotModelN.Axes[0].Maximum = Math.Max(Reactor1PointsN.Max(x => x.X), Reactor2PointsN.Max(x => x.X)) * 1.1;
                PlotModelN.Axes[1].Maximum = Math.Max(Reactor1PointsN.Max(x => x.Y), Reactor2PointsN.Max(x => x.Y)) * 1.1;
                ((LineSeries)PlotModelN.Series[1]).Points.Clear();
                ((LineSeries)PlotModelN.Series[1]).Points.AddRange(Reactor2PointsN);
                PlotModelQ.InvalidatePlot(true);
                PlotModelN.InvalidatePlot(true);
                OnPropertyChanged();
            }
        }
        public Cursor Cursor
        {
            get => _cursor;
            set
            {
                if (_cursor == value)
                    return;
                _cursor = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            System = new ReactorSystem();
            PlotModelQ = new PlotModel() { Title = "Графики зависимости потока выгорания в системе из 2 реакторов" };
            var xAxis = new LinearAxis() { Minimum = 0, Maximum = System.Tmax, Position=AxisPosition.Bottom, FontSize=14};
            var yAxis = new LinearAxis() { Minimum = 0, Maximum = 5, Position=AxisPosition.Left, FontSize = 14 };
            PlotModelQ.Axes.Add(xAxis);
            PlotModelQ.Axes.Add(yAxis);
            PlotModelQ.LegendTitle = "Условные обозначения";
            PlotModelQ.LegendPosition = LegendPosition.RightBottom;
            PlotModelQ.Series.Add(new LineSeries() { Title="1 реактор", FontSize = 14 });
            PlotModelQ.Series.Add(new LineSeries() { Title="2 реактор", FontSize = 14 });

            PlotModelN = new PlotModel() { Title = "Графики зависимости спетра ТВС в системе из 2 реакторов" };
            var xAxis1 = new LinearAxis() { Minimum = 0, Maximum = System.Tmax, Position = AxisPosition.Bottom, FontSize = 14 };
            var yAxis1 = new LinearAxis() { Minimum = 0, Maximum = 5, Position = AxisPosition.Left , FontSize = 14 };
            PlotModelN.Axes.Add(xAxis1);
            PlotModelN.Axes.Add(yAxis1);
            PlotModelN.LegendTitle = "Условные обозначения";
            PlotModelN.LegendPosition = LegendPosition.RightBottom;
            PlotModelN.Series.Add(new LineSeries() { Title = "1 реактор", FontSize = 14 });
            PlotModelN.Series.Add(new LineSeries() { Title = "2 реактор", FontSize = 14 });
        }
        private ICommand _calculateCommand;
        public ICommand CalculateCommand { get => _calculateCommand ?? (_calculateCommand = new BaseCommand(CalculationStart)); }
        private void CalculationStart(object o = null)
        {
            Cursor = Cursors.Wait;
            var sw = new Stopwatch();
            sw.Start();
            System.Calculate();
            sw.Stop();
            Cursor = Cursors.Arrow;
            MessageBox.Show(sw.ElapsedMilliseconds.ToString());
            T = 1;
        }
    }
}
