using Orbit.Models.Adhoc;
using System.Collections.Generic;
using System.Linq;

namespace Orbit.Models.Assets
{
    public class Well : BaseEntity, IAsset
    {
        public virtual string Name => $"{Field}-{WellCode}";
        public virtual Field Field { get; }
        public virtual IEnumerable<DrainagePoint> DrainagePoints { get; }
        public virtual string WellCode { get; }
        public virtual double SurfaceCoordinateX { get; }
        public virtual double SurfaceCoordinateY { get; }
        public virtual WellDeviation WellDeviation => WellDeviations.LastOrDefault(x=>x.Well.Id==Id);
        public virtual IEnumerable<WellDeviation> WellDeviations { get;}
        public override string ToString() => Name;
    }
   
}
