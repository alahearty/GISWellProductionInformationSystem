using Orbit.Models.Assets;
using System.Collections.Generic;
using System.Linq;

namespace Orbit.Application.AssetExplorer
{
    public class OMLExplorerItem: AssetExplorerItem
    {
        public OMLExplorerItem(string oml, IEnumerable<Field> fields)
        {
            DisplayName = $"OML-{oml}";
            Id = oml;
            Fields = fields.Where(i => i.OML == oml).Select((field) => new FieldExplorerItem(field));
        }
        public IEnumerable<FieldExplorerItem> Fields { get; set; }
    }

}
