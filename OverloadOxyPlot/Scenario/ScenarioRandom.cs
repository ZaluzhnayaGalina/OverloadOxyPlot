using System;
using System.Collections.Generic;
using OverloadOxyPlot.Model;
using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.Scenario
{
    public class ScenarioRandom : IScenario
    {
        private IReactor _reactor;
        private IReactor _stoppedReactor;
        private ReactorSystem _reactorSystem;
        public ScenarioRandom(ReactorSystem reactorSystem)
        {
            _stoppedReactor = reactorSystem.Reactors[0];
            _reactor = reactorSystem.Reactors[1];
            _reactorSystem = reactorSystem;
        }
        public double Count { get; set; }
        public double DeltaE { get; set; }
        public int Days { get; set; }
        public IList<IReactor> Reactors { get; set; }

        public void Run()
        {
            Assemblies a = new Assemblies();
            for (int i = 0; i < Days; i++)
            {
                a.Count = Count;
                var E = new Random().Next((int)DeltaE/2,(int)( _stoppedReactor.Em - DeltaE/2));
                a.E1 = E - DeltaE/2;
                a.E2 = E+DeltaE/2;
                var a1 = _stoppedReactor.Remove(a);
                _reactor.Insert(a1);
                _reactorSystem.DayPass();

            }
        }
    }
}
