using System.Collections.Generic;
using System.Linq;

namespace Orbit.Models.Assets
{
    public partial class Reservoir:NamedEntity, IAsset
    {
        public virtual IEnumerable<Well> Wells => DrainagePoints.Select(x => x.Well);
        public virtual IEnumerable<DrainagePoint> DrainagePoints { get; }
        public virtual Field Field {get;}
        public virtual ReservoirTypeEnum ReservoirType {get;}
    }  

}
