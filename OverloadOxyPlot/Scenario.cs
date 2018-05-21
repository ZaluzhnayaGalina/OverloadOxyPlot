using MVVMTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OverloadOxyPlot
{
    class Scenario : BaseNotifyPropertyChanged
    {
        public double Count { get; set; }
        public double DeltaE { get; set; }
        public int SelectedWay { get; set; }
        public int Days { get; set; }
        private ICommand _closeCommand;
        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new BaseCommand(o => ((Window)o).Close()));
        public Scenario()
        {
            Count = 2;
            DeltaE = 100;
            SelectedWay = 0;
            Days = 20;
        }
    }
}
