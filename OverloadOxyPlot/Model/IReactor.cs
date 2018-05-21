using System.Collections.Generic;

namespace OverloadOxyPlot.Model
{
    interface IReactor: IContainer
    {
        double W0 { get; set; }
        double B { get; set; }
        double Q0 { get; set; }

        double KAverage { get; set; }
        double K0 { get; set; }
        double Mef { get; set; }
        List<double> QArray { get; set; }
        void Burn();
        
    }
}
