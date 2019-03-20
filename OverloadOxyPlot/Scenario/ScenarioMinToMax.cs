using System.Collections.Generic;
using OverloadOxyPlot.Model;
using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.Scenario
{
    public class ScenarioMinToMax : IScenario
    {
        private IReactor _reactor;
        private IReactor _stoppedReactor;
        public double Count { get; set; }
        public double DeltaE { get; set; }
        public int Days { get; set; }

        public ScenarioMinToMax(IReactor reactor, IReactor stoppedReactor)
        {
            _reactor = reactor;
            _stoppedReactor = stoppedReactor;
            Reactors = new List<IReactor>();
            Reactors.Add(reactor);
            Reactors.Add(stoppedReactor);
        }

        public void Run()
        {
            Assemblies a = new Assemblies();
            for (int i = 0; i < Days; i++)
            {
                a.Count = Count;
                a.E1 = _stoppedReactor.NArray.FindIndex(x => x > 0) * _stoppedReactor.DeltaE;
                a.E2 = a.E1 + DeltaE;
                var a1 = _stoppedReactor.Remove(a);
                _reactor.Insert(a1);
                _reactor.DayPass();
                _stoppedReactor.DayPass();
                DayPassed?.Invoke( new SystemDayArgsEvents(Reactors, i));

            }
        }

        public IList<IReactor> Reactors { get; set; }
        public event DaySystemEvent DayPassed;
    }
}
    
