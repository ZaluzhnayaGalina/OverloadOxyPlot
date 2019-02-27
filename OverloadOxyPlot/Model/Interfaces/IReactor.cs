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
        List<double> QArray { get; set; }
        void Burn();
        void Fuel();
        int T { get; set; }
        EventHandler<DayEventArgs> DayPassed { get; set; }
        
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
}
