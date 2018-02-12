using System.Collections.Generic;

namespace OverloadOxyPlot.Model
{
    /// <summary>
    ///  Интерфейс контейнера с ТВС
    ///  </summary>
    public interface IContainer
    {
        /// <summary>
        ///  Спектр ТВС
        ///  </summary>
        List<List<double>> NArray { get; set; }
        /// <summary>
        /// Вставка ТВС
        /// </summary>
        /// <param name="assembliesCount"> Число новых ТВС</param>
        /// <param name="energyLow">Нижняя граница энерговыработок новых ТВС</param>
        /// <param name="energyHigh">Верхняя граница новых ТВС</param>
        void Insert(double assembliesCount, double energyLow, double energyHigh);
        /// <summary>
        /// Извлечение ТВС
        /// </summary>
        /// <param name="assembliesCount">Число извлекаемых ТВС</param>
        /// <param name="energyLow"> Нижняя граница энерговыработок извлекаемых ТВС</param>
        /// <param name="energyHigh"> Верхняя граница энерговыработок извлекаемых ТВС</param>
        /// <returns>Число извлеченных ТВС</returns>
        double Remove(double assembliesCount, double energyLow, double energyHigh);
        }
    
}
