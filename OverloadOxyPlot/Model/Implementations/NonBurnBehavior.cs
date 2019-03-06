using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.Model.Implementations
{
    public class NonBurnBehavior: IBurnBehavior
    {
        private readonly IReactor _stoppedReactor;

        public NonBurnBehavior(IReactor stoppedReactor)
        {
            _stoppedReactor = stoppedReactor;
        }

        public void Burn()
        {
            _stoppedReactor.T += 1;
            Fuel();
        }

        public double Fuel()
        {
            return 0;
        }
    }
}