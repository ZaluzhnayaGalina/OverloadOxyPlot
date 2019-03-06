using System;
using System.Collections.Generic;

namespace OverloadOxyPlot.Model.Interfaces
{
    public interface IReactor: IContainer
    {
        double W0 { get; set; }
        double B { get; set; }
        double Q0 { get; set; }

        double KAverage { get; set; }
        double K0 { get; set; }
        double Mef { get; set; }
        double Em { get; set; }
        void DayPass();
        IBurnBehavior BurnBehavior { get; set; }
        int T { get; set; }
        event DayEvent DayPassed;
      

    }
    public class DayEventArgs : EventArgs
    {
        public double Fuel { get; private set; }
        public List<double> N { get; private set; }
        public DayEventArgs(double fuel, List<double> n)
        {
            Fuel = fuel;
            N = new List<double>(n);
        }
    }

    public delegate void DayEvent(object obj, DayEventArgs args);
}
