using System.Collections.Generic;

namespace Orbit.Application.ProductionRate.AverageRate
{
    public class AverageRateDTO
    {
        public IList<AverageRateData> AverageRates { get; set; }
        public string PercentageIncreaseInCondensateRate { get; set; }
        public string PercentageIncreaseInOilRate { get; set; }
        public string PercentageIncreaseInGasRate { get; set; }
        public string PercentageIncreaseInWaterRate { get; set; }
    }
}
