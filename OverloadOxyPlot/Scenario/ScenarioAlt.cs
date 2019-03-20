using System.Collections.Generic;
using OverloadOxyPlot.Model;
using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.Scenario
{
    public class ScenarioAlt : IScenario
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
            Reactors = new List<IReactor>();
            Reactors.Add(reactor);
            Reactors.Add(stoppedReactor);
        }
        public void Run()
        {
            Assemblies a = new Assemblies();
            Assemblies a1;
            bool tmp = true;
            for (int i = 0; i < Days; i++)
            {
                if (_reactor.AssembliesCount + Count < 1670)
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
                _reactor.DayPass();
                _stoppedReactor.DayPass();
                DayPassed?.Invoke(new SystemDayArgsEvents(Reactors, i));
                tmp = !tmp;
            }
        }

        public IList<IReactor> Reactors { get; set; }
        public event DaySystemEvent DayPassed;
    }
}
