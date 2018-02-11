using System;
using System.Collections.Generic;


namespace OverloadOxyPlot.Model
{
    public class ReactorSystem
    {
        private List<double> _listE = new List<double>();
        private double _tmax=30;
        private double _deltaT=0.1;
        private double _deltaE;
        public Reactor Reactor1 { get; set; }
        public  Reactor Reactor2 { get; set; }


        public double Tmax
        {
            get => _tmax;
            set
            {
                _tmax = value;
                
                int n = (int)Math.Ceiling(_tmax / DeltaT);
                Reactor1.ResizeT(n);
                Reactor2.ResizeT(n);

            }
        } //число суток для прогноза

        public double DeltaT
        {
            get =>_deltaT;
            set
            {
                if (value <= 0 || _deltaT == value)
                    return;
                _deltaT = value;
                int n = (int)Math.Ceiling(_tmax / _deltaT);
                Reactor1.ResizeT(n);
                Reactor2.ResizeT(n);
                Reactor1.SetInitialCondition();
                Reactor2.SetInitialCondition();
            }
        }
        public double DeltaE
        {
            get =>_deltaE;
            set
            {
                if (value<= 0 || _deltaE == value)
                    return;
                _deltaE = value;
                int n = (int)Math.Ceiling(Math.Min(Reactor1.Em, Reactor2.Em)/ _deltaE);
                Reactor1.ResizeE(n);
                Reactor2.ResizeE(n);
                Reactor1.SetInitialCondition();
                Reactor2.SetInitialCondition();
                _listE.Clear();
                for (int i = 0; i < n; i++)
                {
                    _listE.Add(i * DeltaE);
                }
            }
        }

        public ReactorSystem()
        {
            Reactor1 = new Reactor();
            Reactor2 = new Reactor();
            Tmax = 30;
            DeltaT = 0.1;
            DeltaE = 1;
            Reactor1.SetInitialCondition();
            Reactor2.SetInitialCondition();
            int m = (int) Math.Ceiling(Math.Min(Reactor1.Em, Reactor2.Em) / DeltaE);
            for (int i = 0; i < m; i++)
            {
                _listE.Add(i*DeltaE);
            }
        }
        public void Calculate()
        {
            var listAlphaReactor1 = new List<double>();
            var listAlphaReactor2 = new List<double>();
            for (int i = 0; i < _listE.Count; i++)
            {
                if (_listE[i] < Reactor1.E1 || _listE[i] > Reactor1.E2)
                    listAlphaReactor1.Add(0);
                else
                {
                    listAlphaReactor1.Add(Reactor1.Alpha);
                }
                if (_listE[i] < Reactor2.E1 || _listE[i] > Reactor2.E2)
                    listAlphaReactor2.Add(0);
                else
                {
                    listAlphaReactor2.Add(Reactor2.Alpha);
                }
            }
            for (int i = 0; i < Reactor1.QArray.Count - 1; i++)
            {
                for (int j = 1; j < Reactor1.QArray[i].Count; j++)
                {
                    Reactor1.QArray[i + 1][j] = Reactor1.QArray[i][j] - (Reactor1.W0 - Reactor1.B * DeltaE * j) * DeltaT / DeltaE *
                        (Reactor1.QArray[i][j] - Reactor1.QArray[i][j - 1]) +
                        (listAlphaReactor2[j] * Reactor2.QArray[i][j] - listAlphaReactor1[j] * Reactor1.QArray[i][j]) * DeltaT;
                    Reactor2.QArray[i + 1][j] = Reactor2.QArray[i][j] - (Reactor2.W0 - Reactor2.B * DeltaE * j) * DeltaT / DeltaE *
                        (Reactor2.QArray[i][j] - Reactor2.QArray[i][j - 1]) +
                        (listAlphaReactor1[j] * Reactor1.QArray[i][j] - listAlphaReactor2[j] * Reactor2.QArray[i][j]) * DeltaT;
                }
            }
            for (int i = 0; i < Reactor1.QArray.Count; i++)
            {
                for (int j = 0; j < Reactor1.QArray[i].Count; j++)
                {
                    Reactor1.NArray[i][j] = Reactor1.QArray[i][j] / (Reactor1.W0 - Reactor1.B * DeltaE * j);
                    Reactor2.NArray[i][j] = Reactor2.QArray[i][j] / (Reactor2.W0 - Reactor2.B * DeltaE * j);
                }
            }
        }
    }
   
}
