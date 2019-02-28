﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using OverloadOxyPlot.Graphics.Interfaces;
using OverloadOxyPlot.Model.Interfaces;
using OxyPlot;

namespace OverloadOxyPlot.Graphics.Implementations
{
    class CustomGraphic : IGraphic, IDataGetter
    {
        public IList<DataPoint> Points { get; set; }
        private Func<DayEventArgs, double> _function;
        public CustomGraphic(Func<DayEventArgs,double> function)
        {
            _function = function;
            Points = new ObservableCollection<DataPoint>();
        }
        public void GetData(object reactor, DayEventArgs eventArgs)
        {
            var ireactor = reactor as IReactor;
            if (ireactor is null)
                return;
            Points.Add(new DataPoint(ireactor.T, _function(eventArgs)));
        }
    }
}
