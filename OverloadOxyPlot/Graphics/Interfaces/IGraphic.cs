using System.Collections.Generic;
using OxyPlot;

namespace OverloadOxyPlot.Graphics.Interfaces
{
    public interface IGraphic
    {
        IList<DataPoint> Points { get; set; }

    }
}
