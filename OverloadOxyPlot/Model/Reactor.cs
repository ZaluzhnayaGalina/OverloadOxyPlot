using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using OverloadOxyPlot.Annotations;

namespace OverloadOxyPlot.Model
{
    public class Reactor: INotifyPropertyChanged
    {
        private BoundaryConditions _boundary;
        public double Em { get; set; }
        public double W0 { get; set; }
        public double B { get; set; }
        public double Q0 { get; set; }
        public double AssembliesCount { get; set; }
        public double KAverage { get; set; }
        public double K0 { get; set; }
        public double E1 { get; set; }
        public double E2 { get; set; }
        public double Alpha { get; set; }
        public List<List<double>> QArray { get; set; } //результат прогноза для потока
        public List<List<double>> NArray { get; set; }
        public BoundaryConditions Boundary
        {
            get => _boundary;
            set
            {
                if (_boundary == value)
                    return;
                _boundary = value;
                SetInitialCondition();
                OnPropertyChanged();
            }
        }

        public void SetInitialCondition()
        {
            if (_boundary == BoundaryConditions.ConstFuelling)
            {
                foreach (var qt in QArray)
                {
                    qt[0] = Q0;
                }
            }
            if (_boundary == BoundaryConditions.NoFuelling)
                foreach (var qt in QArray)
                {
                    qt[0] = 0;
                }
            for (int i=0; i<QArray[0].Count; i++)
            {
                QArray[0][i] = Q0;
            }
            for (int i = 1; i < QArray.Count; i++)
                for (int j = 1; j < QArray[i].Count; j++)
                    QArray[i][j] = 0;
        }

        public void ResizeT(int newCount)
        {
            if (newCount < QArray.Count)
            {
                QArray.RemoveRange(newCount, QArray.Count - newCount);
                NArray.RemoveRange(newCount, QArray.Count - newCount);
            }
            else
            {
                for (int i = QArray.Count; i < newCount; i++)
                {
                    QArray.Add(new List<double>());
                    NArray.Add(new List<double>());
                    for (int j = 0; j < QArray[0].Count; j++)
                    {
                        QArray.Last().Add(0);
                        NArray.Last().Add(0);
                    }
                }
            }
        }

        public void ResizeE(int newCount)
        {
            if (newCount < QArray[0].Count)
            {
                for (int i = 0; i < QArray.Count; i++)
                {
                    QArray[i].RemoveRange(newCount, QArray[i].Count-newCount);
                    NArray[i].RemoveRange(newCount, QArray[i].Count - newCount);
                }
            }
            else
            {
                for (int i = 0; i < QArray.Count; i++)
                {
                    for (int j = QArray[i].Count; j < newCount; j++)
                    {
                        QArray[i].Add(0);
                        NArray[i].Add(0);
                    }
                }
            }
           // SetInitialCondition();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Reactor()
        {
            QArray = new List<List<double>>();
            NArray = new List<List<double>>();
            AssembliesCount = 1600;
            Em = 2800;
            W0 = 3;
            KAverage = 1.02;
            K0 = 1.2;
            const double wMin = 1.5;
            B = (W0 - wMin) / Em;
            Q0 = B * AssembliesCount / Math.Log(W0 / wMin);
            Boundary = BoundaryConditions.ConstFuelling;
            Alpha = 0.1;
            E1 = Em / 2 - 20;
            E2 = Em / 2 + 20;
            

        }
    }
    public enum BoundaryConditions
    {
        [Description("Постоянная подпитка")]
        ConstFuelling=0,
        [Description("Нет подпитки")]
        NoFuelling=1
    }
}
