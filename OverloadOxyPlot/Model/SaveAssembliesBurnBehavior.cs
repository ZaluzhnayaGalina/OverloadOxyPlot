namespace OverloadOxyPlot.Model
{
    class SaveAssembliesBurnBehavior : IBurnBehavior
    {
        public void Burn(IReactor Reactor)
        {
            double[] qTemp = new double[Reactor.QArray.Count];
            Reactor.QArray.CopyTo(qTemp);
            double sum = 0;
            for(var i=1; i<qTemp.Length; i++)
            {
                Reactor.QArray[i] = qTemp[i] + Reactor.DeltaT / Reactor.DeltaE * (qTemp[i - 1] - qTemp[i]);
                Reactor.NArray[i] = Reactor.QArray[i] / (Reactor.W0 - Reactor.B * Reactor.DeltaE * i);
                sum += Reactor.NArray[i];
            }
            Reactor.NArray[0] = Reactor.AssembliesCount / Reactor.DeltaE - sum;
            Reactor.QArray[0] = Reactor.NArray[0] / Reactor.W0;
            Reactor.Q0 = Reactor.QArray[0];
        }
    }
     
}
