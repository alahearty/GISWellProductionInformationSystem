using Orbit.Application.ProductionRate.AverageRate;
using Orbit.Application.ProductionRate.TotalRate;
using System;
using System.Collections.Generic;

namespace Orbit.Application.ProductionRate
{
    public class TotalAvgRate
    {
        public IEnumerable<DateTime?> Dates { get; set; } = new List<DateTime?>();
        public IList<TotalRateData> TotalRates { get; set; } = new List<TotalRateData>();
        public IList<AverageRateData> AverageRates { get; set; } = new List<AverageRateData>();
        public string TotalPercentageIncreaseInCondensateRate { get; set; }
        public string TotalPercentageIncreaseInOilRate { get; set; }
        public string TotalPercentageIncreaseInGasRate { get; set; }
        public string TotalPercentageIncreaseInWaterRate { get; set; }
        public string AvgPercentageIncreaseInCondensateRate { get; set; }
        public string AvgPercentageIncreaseInOilRate { get; set; }
        public string AvgPercentageIncreaseInGasRate { get; set; }
        public string AvgPercentageIncreaseInWaterRate { get; set; }
    }
}
