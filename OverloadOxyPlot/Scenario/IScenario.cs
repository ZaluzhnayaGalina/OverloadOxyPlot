namespace OverloadOxyPlot.Scenario
{
    public interface IScenario
    {
        double Count { get; set; }
        double DeltaE { get; set; }
        int Days { get; set; }
        void Run();
    }
}
