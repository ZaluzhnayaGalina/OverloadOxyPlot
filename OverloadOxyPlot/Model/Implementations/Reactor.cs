using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using OverloadOxyPlot.Annotations;
using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.Model.Implementations
{
    public class Reactor : IReactor, INotifyPropertyChanged
    {
        public double Em { get; set; }

        public double W0 { get; set; }
        public double B { get; set; }
        public double Fuel { get; set; }
        public double AssembliesCount => NArray.Sum() * DeltaE;
        public double[] NArray { get; set; }
        public double DeltaE { get; set; }

        public double DeltaT { get; set; }
        public List<double[]> Protocol { get; set; }

        public double Mef
        {
            get { return _mef; }
            set
            {
                if (Math.Abs(_mef - value) < 1e-5)
                    return;
                _mef = value;
                OnPropertyChanged();
            }
        }

        public event DayEvent DayPassed;
        public int T { get; set; }

        private double _mef;
        public IBurnBehavior BurnBehavior { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Insert(Assemblies assemblies)
        {
            var j1 = (int) Math.Ceiling(assemblies.E1 / DeltaE);
            var j2 = (int) Math.Ceiling(assemblies.E2 / DeltaE);
            {
                for (int j = j1; j <= j2; j++)
                {
                    NArray[j] += assemblies.Count / (assemblies.E2 - assemblies.E1);
                }
            }
            Protocol.Add(NArray);
            // Fuel();
            OnPropertyChanged("AssembliesCount");
        }

        public Assemblies Remove(Assemblies assemblies)
        {
            var j1 = (int) Math.Ceiling(assemblies.E1 / DeltaE);
            var j2 = (int) Math.Ceiling(assemblies.E2 / DeltaE);
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
            OnPropertyChanged("AssembliesCount");
            // Fuel();
            return new Assemblies {Count = alpha * sum, E1 = assemblies.E1, E2 = assemblies.E2};

        }

        public Reactor()
        {
            DeltaE = 1;
            DeltaT = 0.1;
            Protocol = new List<double[]>();
            
            //AssembliesCount = 1600;
            Em = 2800;
            W0 = 3;
            double N = 1660.0;
            const double wMin = 1.5;
            B = (W0 - wMin) / Em;
            Fuel = B * N / Math.Log(W0 / wMin);
            NArray = new double[(int)Math.Ceiling(Em / DeltaE)];
            for (int i = 0; i < Em / DeltaE; i++)
            {
                NArray[i]=Fuel / (W0 - B * DeltaE * i);
            }
            Protocol.Add(NArray);
        }

        public void DayPass()
        {
            BurnBehavior.Burn();
            Fuel = BurnBehavior.Fuel();
            var fuelled = new ReactorDayEventArgs(Fuel, NArray);
            DayPassed?.Invoke(this, fuelled);
        }

    }
}
