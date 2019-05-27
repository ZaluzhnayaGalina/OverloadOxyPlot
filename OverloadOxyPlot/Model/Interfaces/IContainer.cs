using System.Collections.Generic;

namespace OverloadOxyPlot.Model.Interfaces
{
    /// <summary>
    ///  Интерфейс контейнера с ТВС
    ///  </summary>
    public interface IContainer
    {
        /// <summary>
        ///  Спектр ТВС
        ///  </summary>
        double[] NArray { get; set; }
        /// <summary>
        /// Вставка ТВС
        /// </summary>
        /// <param name="assemblies"> Число новых ТВС</param>
        void Insert(Assemblies assemblies);
        /// <summary>
        /// Извлечение ТВС
        /// </summary>
        /// <param name="assemblies">Число извлекаемых ТВС</param>
        /// <returns>Число извлеченных ТВС</returns>
        Assemblies Remove(Assemblies assemblies);
        /// <summary>
        /// Шаг по энерговыработкам
        /// </summary>
        /// 
        double DeltaE { get; set; }
        double DeltaT { get; set; }
        List<double[]> Protocol { get; set; }
        double AssembliesCount { get; }

    }   
}
