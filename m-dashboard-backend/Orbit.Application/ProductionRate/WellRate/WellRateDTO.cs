using System;

namespace Orbit.Application.ProductionRate.WellRate
{
    public class WellRateDTO {
        public string AssetName { get; set; }
        public double? CurrentCondensateRate { get; set; }
        public double? CurrentGasRate { get; set; }
        public double? CurrentGasRateBOE { get; set; }
        public double? CurrentOilRate { get; set; }
        public double? CurrentWaterRate { get; set; }
        public DateTime? CurrentOilDate { get; set; }
        public DateTime? CurrentGasDate { get; set; }
        public DateTime? CurrentWaterDate { get; set; }
        public DateTime? CurrentCondDate { get; set; }

        public double? PreviousCondensateRate { get; set; }
        public double? PreviousOilRate { get; set; }
        public double? PreviousGasRate { get; set; }
        public double? PreviousWaterRate { get; set; }
        public DateTime? PreviousOilDate { get; set; }
        public DateTime? PreviousGasDate { get; set; }
        public DateTime? PreviousWaterDate { get; set; }
        public DateTime? PreviousCondDate { get; set; }

        public string PercentageIncreaseInCondensateRate { get; set; }
        public string PercentageIncreaseInOilRate { get; set; }
        public string PercentageIncreaseInGasRate { get; set; }
        public string PercentageIncreaseInWaterRate { get; set; }
    }
}
