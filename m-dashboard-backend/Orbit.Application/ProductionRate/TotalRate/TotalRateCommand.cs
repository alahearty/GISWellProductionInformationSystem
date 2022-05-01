using Orbit.Models;
using System;
using System.Collections.Generic;

namespace Orbit.Application.ProductionRate.TotalRate
{
    public class TotalRateCommand
    {
        public DateTime Date { get; set; }
        public IEnumerable<string> Ids { get; set; } = new List<string>();
        public AssetType AssetType { get; set; }
    }
}
