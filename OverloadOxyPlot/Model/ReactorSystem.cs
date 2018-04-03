using System;
using System.Collections.Generic;


namespace OverloadOxyPlot.Model
{
    //public class ReactorSystem
    //{
    //    private List<double> _listE = new List<double>();
    //    private double _tmax=30;
    //    private double _deltaT=0.1;
    //    private double _deltaE;
    //    public Reactor Reactor1 { get; set; }
    //    public  Reactor Reactor2 { get; set; }


    //    public double Tmax
    //    {
    //        get => _tmax;
    //        set
    //        {
    //            _tmax = value;
                
    //            int n = (int)Math.Ceiling(_tmax / DeltaT);
    //            Reactor1.ResizeT(n);
    //            Reactor2.ResizeT(n);

    //        }
    //    } //число суток для прогноза

    //    public double DeltaT
    //    {
    //        get =>_deltaT;
    //        set
    //        {
    //            if (value <= 0 || _deltaT == value)
    //                return;
    //            _deltaT = value;
    //            int n = (int)Math.Ceiling(_tmax / _deltaT);
    //            Reactor1.ResizeT(n);
    //            Reactor2.ResizeT(n);
    //            Reactor1.SetInitialCondition();
    //            Reactor2.SetInitialCondition();
    //        }
    //    }
    //    public double DeltaE
    //    {
    //        get =>_deltaE;
    //        set
    //        {
    //            if (value<= 0 || _deltaE == value)
    //                return;
    //            _deltaE = value;
    //            int n = (int)Math.Ceiling(Math.Min(Reactor1.Em, Reactor2.Em)/ _deltaE);
    //            Reactor1.ResizeE(n);
    //            Reactor2.ResizeE(n);
    //            Reactor1.SetInitialCondition();
    //            Reactor2.SetInitialCondition();
    //            _listE.Clear();
    //            for (int i = 0; i < n; i++)
    //            {
    //                _listE.Add(i * DeltaE);
    //            }
    //        }
    //    }

    //    public ReactorSystem()
    //    {
    //        Reactor1 = new Reactor();
    //        Reactor2 = new Reactor();
    //        Tmax = 30;
    //        DeltaT = 0.1;
    //        DeltaE = 1;
    //        Reactor1.SetInitialCondition();
    //        Reactor2.SetInitialCondition();
    //        int m = (int) Math.Ceiling(Math.Min(Reactor1.Em, Reactor2.Em) / DeltaE);
    //        for (int i = 0; i < m; i++)
    //        {
    //            _listE.Add(i*DeltaE);
    //        }
    //    }
    //    public void Calculate()
    //    {
    //        Reactor1.SetInitialCondition();
    //        Reactor2.SetInitialCondition();
    //        Reactor1.DeltaE = DeltaE;
    //        Reactor2.DeltaE = DeltaE;
    //        for (int i = 0; i < Reactor1.QArray.Count - 1; i++)
    //        {
    //            Reactor1.CurrentQArray = Reactor1.QArray[i];
    //            Reactor2.CurrentQArray = Reactor2.QArray[i];
    //            Reactor1.CurrentNArray = Reactor1.NArray[i];
    //            Reactor2.CurrentNArray = Reactor2.NArray[i];
    //            var n1 = Reactor1.Remove(Reactor1.Alpha, Reactor1.E1, Reactor1.E2);
    //            Reactor2.Insert(n1, Reactor1.E1, Reactor1.E2);
    //            var n2 = Reactor2.Remove(Reactor2.Alpha, Reactor2.E1, Reactor2.E2);
    //            Reactor1.Insert(n2, Reactor2.E1, Reactor2.E2);
    //            Reactor1.QArray[i] = Reactor1.CurrentQArray;
    //            Reactor2.QArray[i] = Reactor2.CurrentQArray;
    //            for (int j = 1; j < Reactor1.QArray[i].Count; j++)
    //            {
    //                Reactor1.QArray[i + 1][j] = Reactor1.QArray[i][j] +
    //                                            DeltaT / DeltaE * (-Reactor1.QArray[i][j] + Reactor1.QArray[i][j - 1]);
    //                Reactor2.QArray[i + 1][j] = Reactor2.QArray[i][j] +
    //                                            DeltaT / DeltaE * (-Reactor2.QArray[i][j] + Reactor2.QArray[i][j - 1]);
    //            }
    //        }
    //        for (int i = 0; i < Reactor1.QArray.Count; i++)
    //        {
    //            for (int j = 0; j < Reactor1.QArray[i].Count; j++)
    //            {
    //                Reactor1.NArray[i][j] = Reactor1.QArray[i][j] / (Reactor1.W0 - Reactor1.B * DeltaE * j);
    //                Reactor2.NArray[i][j] = Reactor2.QArray[i][j] / (Reactor2.W0 - Reactor2.B * DeltaE * j);
    //            }
    //        }
    //    }
    //}
   
}
