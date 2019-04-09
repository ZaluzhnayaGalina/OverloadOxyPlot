using System.Collections.Generic;
using OverloadOxyPlot.Model;
using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.Scenario
{
    public interface IScenario
    {
        double Count { get; set; }
        double DeltaE { get; set; }
        int Days { get; set; }
        void Run();
        IList<IReactor> Reactors { get; set; }
    }
}

