using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using OverloadOxyPlot.Annotations;
using OverloadOxyPlot.Model;
using OverloadOxyPlot.Model.Interfaces;

namespace OverloadOxyPlot.Scenario
{
    public class ScenarioCreator: INotifyPropertyChanged
    {
        private double _count;
        private double _deltaE;
        private int _days;

        public double Count
        {
            get => _count;
            set
            {
                if (Math.Abs(_count - value) < 1e-5)
                    return;
                _count = value;
                OnPropertyChanged();
            }
        }

        public double DeltaE
        {
            get => _deltaE;
            set
            {
                if (Math.Abs(_deltaE - value) < 1e-5)
                    return;
                _deltaE = value;
                OnPropertyChanged();
            }
        }

        public int Days
        {
            get { return _days; }
            set
            {
                if (_days == value)
                    return;
                _days = value;
                OnPropertyChanged();
            }
        }
        public  ScenarioTypes ScenarioType { get; set; }

        public IScenario CreateScenario(ReactorSystem reactorSystem)
        {
            if(ScenarioType == ScenarioTypes.Alt)
                return new ScenarioAlt(reactorSystem) {Count = Count, Days = Days, DeltaE = DeltaE};
            if(ScenarioType==ScenarioTypes.MinToMax)
                return new ScenarioMinToMax(reactorSystem) { Count = Count, Days = Days, DeltaE = DeltaE };
            return null;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
