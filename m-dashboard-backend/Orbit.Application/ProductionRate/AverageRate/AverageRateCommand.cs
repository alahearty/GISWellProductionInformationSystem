using Orbit.Models;
using System.Collections.Generic;

namespace Orbit.Application.ProductionRate.AverageRate
{
    public class AverageRateCommand
    {
        public IEnumerable<string> Ids { get; set; } = new List<string>();
        public AssetType AssetType { get; set; }
    }
}
