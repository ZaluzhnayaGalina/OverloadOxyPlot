using System;
using System.Collections.Generic;

namespace OverloadOxyPlot.Model.Interfaces
{
    public interface IReactor: IContainer
    {
        double W0 { get; set; }
        double B { get; set; }
        double Fuel { get; set; }
        double Mef { get; set; }
        double Em { get; set; }
        void DayPass();
        IBurnBehavior BurnBehavior { get; set; }
        int T { get; set; }
        event DayEvent DayPassed;
      

    }
    public class ReactorDayEventArgs : EventArgs
    {
        public double Fuel { get; }
        public List<double> N { get; }
        public ReactorDayEventArgs(double fuel, List<double> n)
        {
            Fuel = fuel;
            N = new List<double>(n);
        }
    }

    public delegate void DayEvent(object obj, ReactorDayEventArgs args);
}
