using OverloadOxyPlot.Model.Interfaces;
using OxyPlot;
using System.Collections.Generic;

namespace OverloadOxyPlot.Graphics
{
    interface IGraphic
    {
        IList<DataPoint> Points { get; set; }
        void GetData(IReactor reactor, DayEventArgs eventArgs);
    }
}
