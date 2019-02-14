using System;
using OverloadOxyPlot.Model;
using OverloadOxyPlot.Model.Implementations;
using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.Scenario
{
    class ScenarioMinToMax : IScenario
    {
        private double _fuel;
        private IReactor _reactor;
        private IReactor _stoppedReactor;
        public double Count { get; set; }
        public double DeltaE { get; set; }
        public int Days { get; set; }
        private string _description = "От свежего к выгоревшему";
        public string Description
        {
            get => _description;
            set
            {
                _description = value;                
            }
        }

        public ScenarioMinToMax(IReactor reactor, IReactor stoppedReactor)
        {
            _reactor = reactor;
            _stoppedReactor = stoppedReactor;
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
                for (int j = 0; j < 1.0 / _reactor.DeltaT; j++)
                {
                    _reactor.Burn();
                    _stoppedReactor.Burn();
                }
                _reactor.Fuel();
                _stoppedReactor.Fuel();
            }
        }
    }
}
    
