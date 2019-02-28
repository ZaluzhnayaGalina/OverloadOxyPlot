using OverloadOxyPlot.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OverloadOxyPlot.Model
{
    public class Assemblies : INotifyPropertyChanged
    {
        private double _count;
        private double _e1;
        private double _e2;

        public double Count
        {
            get => _count;
            set
            {
                if (Math.Abs(_count - value) < 0.001)
                    return;
                _count = value;
                OnPropertyChanged();
            }
        }

        public double E1
        {
            get => _e1;
            set
            {
                if (Math.Abs(_e1 - value) < 0.001)
                    return;
                _e1 = value;
                OnPropertyChanged();
            }
        }

        public double E2
        {
            get => _e2;
            set
            {
                if (Math.Abs(_e2 - value) < 0.001)
                    return;
                _e2 = value;
                OnPropertyChanged();
            }
        }
        public Assemblies(double count, double e1, double e2)
        {
            Count = count;
            E1 = e1;
            E2 = e2;
        }
        public Assemblies()
        { }
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
