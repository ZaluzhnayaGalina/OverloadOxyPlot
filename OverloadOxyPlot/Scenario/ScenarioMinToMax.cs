﻿using System.Collections.Generic;
using OverloadOxyPlot.Model;
using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.Scenario
{
    public class ScenarioMinToMax : IScenario
    {
        private IReactor _reactor;
        private IReactor _stoppedReactor;
        private ReactorSystem _reactorSystem;
        public double Count { get; set; }
        public double DeltaE { get; set; }
        public int Days { get; set; }

        public ScenarioMinToMax(ReactorSystem reactorSystem)
        {
            _stoppedReactor = reactorSystem.Reactors[0];
            _reactor = reactorSystem.Reactors[1];
            _reactorSystem = reactorSystem;
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
                _reactorSystem.DayPass();

            }
        }

        public IList<IReactor> Reactors { get; set; }
        public event DaySystemEvent DayPassed;
    }
}
    
