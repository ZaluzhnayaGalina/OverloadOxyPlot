using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using OverloadOxyPlot.Annotations;

namespace OverloadOxyPlot.Model
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
        public double Mef { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Burn()
        {
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
    public class Assemblies : INotifyPropertyChanged
    {
        private double _count;
        private double _e1;
        private double _e2;

        public double Count
        {
            get => _count;
            set
            {
                if (Math.Abs(_count - value) < 0.001)
                    return;
                _count = value;
                OnPropertyChanged();
            }
        }

        public double E1
        {
            get => _e1;
            set
            {
                if (Math.Abs(_e1 - value) < 0.001)
                    return;
                _e1 = value;
                OnPropertyChanged();
            }
        }

        public double E2
        {
            get => _e2;
            set
            {
                if (Math.Abs(_e2 - value) < 0.001)
                    return;
                _e2 = value;
                OnPropertyChanged();
            }
        }
        public Assemblies(double count, double e1, double e2)
        {
            Count = count;
            E1 = e1;
            E2 = e2;
        }
        public Assemblies()
        { }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}