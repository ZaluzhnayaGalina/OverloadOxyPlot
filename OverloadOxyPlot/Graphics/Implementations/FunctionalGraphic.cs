using System.Collections.Generic;
using OverloadOxyPlot.Graphics.Interfaces;
using OverloadOxyPlot.Scenario;
using OxyPlot;

namespace OverloadOxyPlot.Graphics.Implementations
{
    class FunctionalGraphic: IGraphic, ISystemDataGetter
    {
        public IList<DataPoint> Points { get; set; }
        public void GetData(SystemDayArgsEvents eventArgs)
        {
            var reactors = eventArgs.Reactors;
        }
    }
}
