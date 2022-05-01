using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.ProductionRate.TotalRate
{
    public class TotalRateDTO
    {
        public IEnumerable<DateTime?> Dates { get; set; }
        public IList<TotalRateData> TotalRates { get; set; }
        public string PercentageIncreaseInCondensateRate { get; set; }
        public string PercentageIncreaseInOilRate { get; set; }
        public string PercentageIncreaseInGasRate { get; set; }
        public string PercentageIncreaseInWaterRate { get; set; }
    }
}
