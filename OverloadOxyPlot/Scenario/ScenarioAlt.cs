using System.Collections.Generic;
using OverloadOxyPlot.Model;
using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.Scenario
{
    public class ScenarioAlt : IScenario
    {
        private IReactor _reactor;
        private IReactor _stoppedReactor;
        private ReactorSystem _reactorSystem;
        public double Count { get ; set ; }
        public double DeltaE { get; set ; }
        public int Days { get; set; }

        public ScenarioAlt(ReactorSystem reactorSystem)
        {
            _stoppedReactor = reactorSystem.Reactors[0];
            _reactor = reactorSystem.Reactors[1];
            _reactorSystem = reactorSystem;
        }
        public void Run()
        {
            Assemblies a = new Assemblies();
            Assemblies a1;
            bool tmp = true;
            for (int i = 0; i < Days; i++)
            {
                a.Count = Count;
                if (tmp)
                {
                    int id0;
                    for (id0 = 0; id0 < _stoppedReactor.NArray.Length; id0++)
                    {
                        if (_stoppedReactor.NArray[id0] > 0)
                            break;
                    }
                    a.E1 = id0 * _stoppedReactor.DeltaE;
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
                    int id0;
                    for (id0=0; id0<_stoppedReactor.NArray.Length;id0++)
                        {
                            if (_stoppedReactor.NArray[id0] > 0)
                                break;
                        }
                        int k = id0 + (_stoppedReactor.NArray.Length - id0) / 2;
                        a.E2 = k * _stoppedReactor.DeltaE + DeltaE / 2;
                        a.E1 = a.E2 - DeltaE;
                    }
                    a1 = _stoppedReactor.Remove(a);

                    _reactor.Insert(a1);
                
                _reactorSystem.DayPass();
                tmp = !tmp;
            }
        }

        public IList<IReactor> Reactors { get; set; }
    }
}
