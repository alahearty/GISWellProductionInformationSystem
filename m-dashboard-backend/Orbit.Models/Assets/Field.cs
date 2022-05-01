using System.Collections.Generic;
using System.Linq;

namespace Orbit.Models.Assets
{
    public class Field: NamedEntity,IAsset
    {
        public virtual IEnumerable<Reservoir> Reservoirs { get; }

        public virtual IEnumerable<Well> Wells { get; }

        public virtual IEnumerable<DrainagePoint> DrainagePoints => Wells.SelectMany(x => x.DrainagePoints);
        public virtual string OML { get; }
    }
}
