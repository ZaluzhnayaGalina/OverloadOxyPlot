using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.Graphics.Interfaces
{
    public interface IDataGetter
    {
        void GetData(object reactor, ReactorDayEventArgs eventArgs);
    }
}
