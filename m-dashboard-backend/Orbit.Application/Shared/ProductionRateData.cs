using System;

namespace Orbit.Application.Shared
{
    public struct ProductionRateData
    {
        public string DrainagePointName { get; set; }
        public double? OilRate { get; set; }
        public double? GasRate { get; set; }
        public double? WaterRate { get; set; }
        public double? CondensateRate { get; set; }
        public DateTime? Date { get; set; }
    }
}
