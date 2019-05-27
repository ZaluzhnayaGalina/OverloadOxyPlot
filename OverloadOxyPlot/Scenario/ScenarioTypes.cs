using System.ComponentModel;

namespace OverloadOxyPlot.Scenario
{
    public enum ScenarioTypes
    {
        [Description("От минимума к максимуму")]
       MinToMax = 0,
        [Description("Попеременно")]
       Alt = 1,
        [Description("Случайная перегрузка")]
        Random=2
    }
}
