using OverloadOxyPlot.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverloadOxyPlot.Scenario
{
    class ScenarioAlt : IScenario
    {
        private IReactor _reactor;
        private IReactor _stoppedReactor;
        public double Count { get ; set ; }
        public double DeltaE { get; set ; }
        public int Days { get; set; }
        public ScenarioAlt(IReactor reactor, IReactor stoppedReactor)
        {
            _reactor = reactor;
            _stoppedReactor = stoppedReactor;
        }
        public void Run()
        {
            Assemblies a = new Assemblies();
            Assemblies a1 = new Assemblies();
            bool tmp = true;
            for (int i = 0; i < Days; i++)
            {
                if (_reactor.AssembliesCount + Count < 1668)
                {
                    a.Count = Count;
                    if (tmp)
                    {
                        a.E1 = _stoppedReactor.NArray.FindIndex(x => x > 0) * _stoppedReactor.DeltaE;
                        a.E2 = a.E1 + DeltaE;
                    }
                    else
                    {
                        //for (int k = StoppedReactor.NArray.Count - 1; k >= 0; k--)
                        //{
                        //    if (Math.Abs(StoppedReactor.NArray[k]) > 0.01)
                        //    {
                        //        a.E2 = k * StoppedReactor.DeltaE;
                        //        break;
                        //    }
                        //}
                        int id0 = _stoppedReactor.NArray.FindIndex(x => x > 0);
                        int k = id0 + (_stoppedReactor.NArray.Count - id0) / 2;
                        a.E2 = k * _stoppedReactor.DeltaE + DeltaE / 2;
                        a.E1 = a.E2 - DeltaE;
                    }
                    a1 = _stoppedReactor.Remove(a);

                    _reactor.Insert(a1);
                }
                for (int j = 0; j < 1.0 / _reactor.DeltaT; j++)
                {
                    _reactor.Burn();
                    _stoppedReactor.Burn();
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
                tmp = !tmp;
                //ProgressBarValue = (double)(i + 1) / (double)_scenario.Days * 100;
            }
        }
    }
}
