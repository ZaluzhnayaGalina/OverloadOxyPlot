using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.Model.Implementations
{
    public class BurnBehavior: IBurnBehavior
    {
        private readonly IReactor _burningReactor;
        public double A;
        public static double A2 = 0.06;
        public static double M = 0.2;
        public static double BesselConst = 2.405;
        public double KAverage { get; set; }
        public double K0 { get; set; }
        public BurnBehavior(IReactor burningReactor)
        {
            KAverage = 1.02;
            K0 = 1.2;
            _burningReactor = burningReactor;
            int j = 0;
            double eAv = _burningReactor.NArray.Sum(x => x * _burningReactor.DeltaE * j++) / _burningReactor.NArray.Sum();
            A = (K0 - KAverage) / eAv;
            j = 0;
            var kinf = _burningReactor.NArray.Sum(x => x * (K0 - A * _burningReactor.DeltaE * j++)) / _burningReactor.NArray.Sum();
            var r = Math.Sqrt(A2 * _burningReactor.AssembliesCount / Math.PI);
            _burningReactor.Mef = kinf / (1 + Math.Pow(M * 2.405 / r, 2.0));
        }

        public void Burn()
        {
            for (int i = 0; i < 1.0 / _burningReactor.DeltaT; i++)
            {
                var prev = _burningReactor.Protocol.Last();
                int nArrayCount = _burningReactor.NArray.Length;
                _burningReactor.NArray = new double[nArrayCount];
                Parallel.For(1, nArrayCount, j =>
                  {
                      _burningReactor.NArray[j] = prev[j] + _burningReactor.DeltaT *
                              ((_burningReactor.W0 - _burningReactor.B * j * _burningReactor.DeltaE) / _burningReactor.DeltaE * (-prev[j] + prev[j - 1]) + _burningReactor.B * prev[j]);
                  }
                );
                _burningReactor.Protocol.Add(_burningReactor.NArray);
            }
        }

        public double Fuel()
        {
            var fuel = 0.0;
            var a = new Assemblies();
            while (_burningReactor.AssembliesCount >= MaxAssembliesCount)
            {
                RemoveBurntAssemblies(a);
            }
            int j = 0;
            double kinf = _burningReactor.NArray.Sum(x => x * (K0 - A * _burningReactor.DeltaE * j++)) / _burningReactor.NArray.Sum();
            double r = Math.Sqrt(A2 * _burningReactor.AssembliesCount / Math.PI);
            double keff = kinf / (1 + Math.Pow(M * BesselConst / r, 2.0));
            const double minFreshCount = 0.01;
            var freshAssemblies = new Assemblies(minFreshCount, 0.0, 1);
            while (keff < 1.0125)
            {
                _burningReactor.Insert(freshAssemblies);
                fuel += minFreshCount;
                j = 0;
                kinf = _burningReactor.NArray.Sum(x => x * (K0 - A * _burningReactor.DeltaE * j++)) / _burningReactor.NArray.Sum();
                r = Math.Sqrt(A2 * _burningReactor.AssembliesCount / Math.PI);
                keff = kinf / (1 + Math.Pow(M * BesselConst / r, 2.0));

            }
            _burningReactor.Mef = keff;
            _burningReactor.T += 1;
            return fuel;
        }

        private void RemoveBurntAssemblies(Assemblies a)
        {
            for (int k = _burningReactor.NArray.Length - 1; k >= 0; k--)
            {
                if (Math.Abs(_burningReactor.NArray[k]) > 0.01)
                {
                    a.E2 = k * _burningReactor.DeltaE;
                    break;
                }
            }
            a.E1 = a.E2 - 1;
            a.Count = 0.001;
            _burningReactor.Remove(a);
        }

        public double MaxAssembliesCount = 1670;
    }
}