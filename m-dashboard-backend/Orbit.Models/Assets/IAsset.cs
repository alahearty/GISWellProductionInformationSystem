using System.Collections.Generic;

namespace Orbit.Models.Assets
{
    public interface IAsset
    {
       IEnumerable<DrainagePoint> DrainagePoints { get; }
       string Name { get; }
    }
}
