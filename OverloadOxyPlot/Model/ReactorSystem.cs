using OverloadOxyPlot.Model.Interfaces;
using System;
using System.Collections.Generic;

namespace OverloadOxyPlot.Model
{
    public class ReactorSystem
    {
        public IList<IReactor> Reactors { get; set; }
        public int T { get; set; }
        public event DaySystemEvent DayPassed;
        public ReactorSystem()
        {
            Reactors = new List<IReactor>();
            T = 0;
        }
        public void DayPass()
        {
            foreach(var reactor in Reactors)
            {
                reactor.DayPass();
            }
            DayPassed?.Invoke(new SystemDayArgsEvent(Reactors, T));
            T++;
        }
    }
    public class SystemDayArgsEvent : EventArgs
    {
        public IList<IReactor> Reactors { get; }
        public int T { get; }
        public SystemDayArgsEvent(IList<IReactor> reactors, int t)
        {
            Reactors = reactors;
            T = t;
        }
    }

    public delegate void DaySystemEvent(SystemDayArgsEvent eventArgs);
}
