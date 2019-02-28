using System;
using System.Collections.Generic;
using System.ComponentModel;
using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.Model.Implementations
{
    public class StoppedReactor : IReactor, INotifyPropertyChanged
    {
        public List<double> NArray { get; set; }


        public void Insert(Assemblies assemblies)
        {
            var j1 = (int)Math.Ceiling(assemblies.E1 / DeltaE);
            var j2 = (int)Math.Ceiling(assemblies.E2 / DeltaE);
            double sum = 0;
            for (int j = j1; j <= j2; j++)
            {
                sum += NArray[j];
            }
            sum = sum * DeltaE;
            var alpha = assemblies.Count / sum;
            for (int j = j1; j <= j2; j++)
            {
                NArray[j] += alpha * NArray[j];
            }
            Protocol.Add(NArray);
        }

        public Assemblies Remove(Assemblies assemblies)
        {
            var j1 = (int)Math.Ceiling(assemblies.E1 / DeltaE);
            var j2 = (int)Math.Ceiling(assemblies.E2 / DeltaE);
            double sum = 0;
            for (int j = j1; j <= j2; j++)
            {
                sum += NArray[j] * DeltaE;
            }
            var alpha = assemblies.Count / sum; //TODO; а что если там нет ТВС и сумма = 0?
            if (alpha > 1)
                alpha = 1;
            for (int j = j1; j <= j2; j++)
            {
                NArray[j] -= alpha * NArray[j];
            }
            Protocol.Add(NArray);
            return new Assemblies{ Count = alpha * sum, E1 = assemblies.E1, E2 = assemblies.E2 };

        }

        public double Em { get; set; }
        public double DeltaE { get; set; }
        public double DeltaT { get; set; }
        public List<List<double>> Protocol { get; set; }

        public double W0 { get; set; }
        public double B { get; set; }
        public double Q0 { get; set; }
        public double AssembliesCount { get; set; }
        public double KAverage { get; set; }
        public double K0 { get; set; }
        public List<double> QArray { get; set; }
        public double Mef { get; set; }
        public event DayEvent DayPassed;
        public int T { get; set; }

        public void Burn()
        {
            T += 1;
            Fuel();
        }

        public void Fuel()
        {
            DayPassed?.Invoke(this,new DayEventArgs(0, NArray));
        }

        public StoppedReactor()
        {
            DeltaE = 1;
            DeltaT = 0.1;
            QArray = new List<double>();
            Protocol = new List<List<double>>();
            NArray = new List<double>();
            AssembliesCount = 1600;
            Em = 2800;
            W0 = 3;
            KAverage = 1.02;
            K0 = 1.2;
            const double wMin = 1.5;
            B = (W0 - wMin) / Em;
            Q0 = B * AssembliesCount / Math.Log(W0 / wMin);
            for (int i = 0; i < Em / DeltaE; i++)
            {
                NArray.Add(Q0 / (W0 - B * DeltaE * i));
            }
            Protocol.Add(NArray);
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
   
}