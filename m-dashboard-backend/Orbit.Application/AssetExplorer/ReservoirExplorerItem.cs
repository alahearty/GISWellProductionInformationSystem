using Orbit.Models.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbit.Application.AssetExplorer
{
    public class ReservoirExplorerItem:AssetExplorerItem
    {
        public ReservoirExplorerItem(Reservoir reservoir)
        {
            Id = reservoir.Id.ToString();
            DisplayName = reservoir.Name;
            DrainagePoints = reservoir.DrainagePoints.Select(dp => new DrainagePointExplorerItem(dp));
        }
        public IEnumerable<DrainagePointExplorerItem> DrainagePoints { get; set; }
    }
}
