using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Application.AssetExplorer
{
    public abstract class AssetExplorerItem
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
    }
}
