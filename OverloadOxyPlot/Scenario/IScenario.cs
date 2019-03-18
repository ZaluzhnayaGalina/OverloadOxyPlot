using System;
using System.Collections.Generic;
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
        event DaySystemEvent DayPassed;
    }
    public class SystemDayArgsEvents : EventArgs
    {
        public IList<IReactor> Reactors { get; }
        public int T { get; }
        public SystemDayArgsEvents(IList<IReactor> reactors, int t)
        {
            Reactors = reactors;
            T = t;
        }
    }

    public delegate void DaySystemEvent(SystemDayArgsEvents eventArgs);
}

