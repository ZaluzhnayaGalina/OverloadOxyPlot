﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using OverloadOxyPlot.Model.Interfaces;
using OxyPlot;

namespace OverloadOxyPlot.Graphics
{
    class SpectrumGraphic : IGraphic
    {
        public IList<DataPoint> Points { get; set; } = new ObservableCollection<DataPoint>();

        public void GetData(IReactor reactor, DayEventArgs eventArgs)
        {
            Points.Clear();
            for (int j = 0; j < reactor.NArray.Count; j++)
                Points.Add(new DataPoint(j * reactor.DeltaE, reactor.NArray[j]));
        }
    }
}
