using Orbit.Models.Assets;
using System.Collections.Generic;
using System.Linq;

namespace Orbit.Application.AssetExplorer
{
    public class FieldExplorerItem:AssetExplorerItem
    {
        public FieldExplorerItem(Field field)
        {
            Id = field.Id.ToString();
            DisplayName = field.Name;
            Reservoirs = field.Reservoirs.Select(rev => new ReservoirExplorerItem(rev));
        }
        public IEnumerable<ReservoirExplorerItem> Reservoirs { get; set; }
    }

}
