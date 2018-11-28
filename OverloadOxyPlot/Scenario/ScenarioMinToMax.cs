using OverloadOxyPlot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadOxyPlot.Scenario
{
    class ScenarioMinToMax : IScenario
    {
        private double _fuel;
        private IReactor _reactor;
        private IReactor _stoppedReactor;
        public double Count { get; set; }
        public double DeltaE { get; set ; }
        public int Days { get; set; }
        public ScenarioMinToMax(IReactor reactor, IReactor stoppedReactor)
        {
            _reactor = reactor;
            _stoppedReactor = stoppedReactor;
            _reactor.FuelEvent += _reactor_FuelEvent;
        }

        private void _reactor_FuelEvent(object sender, FuelEventArgs fuelling)
        {
            _fuel += fuelling.Fuelling.Count;
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
            }
                FuellingPoints.Add(new DataPoint(_day, _fuel));
                ConstFuellingPoints.Add(new DataPoint(_day, Reactor.Q0));
                var r1 = GetUnusedResources(Reactor);
                var r2 = GetUnusedResources(StoppedReactor);
                UnusedResource1.Add(new DataPoint(_day, r1));
                UnusedResource2.Add(new DataPoint(_day, r2));
                TotalUnusedResource.Add(new DataPoint(_day, r1 + r2));
                _day++;
                _fuel = 0;
                //ProgressBarValue = (double)(i + 1) / (double)_scenario.Days * 100;
            }
        }
        private double GetUnusedResources(IReactor reactor)
        {
            double sum = 0;
            for (int i = 0; i < reactor.NArray.Count; i++)
            {
                if (i * reactor.DeltaE > reactor.Em)
                    break;
                sum += (reactor.Em - i * reactor.DeltaE) * reactor.NArray[i];
            }
            return sum * reactor.DeltaE / reactor.Em;
        }
    }
}
