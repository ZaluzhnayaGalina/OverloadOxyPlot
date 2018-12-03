using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadOxyPlot.Scenario
{
    interface IScenario
    {
        double Count { get; set; }
        double DeltaE { get; set; }
        int Days { get; set; }
        string Description { get; set; }
        void Run();
    }
}
