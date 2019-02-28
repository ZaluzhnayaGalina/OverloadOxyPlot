using OverloadOxyPlot.Model;
using MVVMTools;
using System.Collections.Generic;

namespace OverloadOxyPlot
{
    internal class MainViewModel : BaseNotifyPropertyChanged
    {
        public IList<Assemblies> AssembliesList { get; set; }
        private Assemblies _assemblies;
        public Assemblies Assemblies
        {
            get => _assemblies;
            set
            {
                if (value==_assemblies)
                    return;
                _assemblies = value;
                OnPropertyChanged();
            }
        }


    }  
}
