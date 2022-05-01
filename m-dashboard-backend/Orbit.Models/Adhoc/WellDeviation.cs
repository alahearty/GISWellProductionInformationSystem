using Orbit.Models.Assets;

namespace Orbit.Models.Adhoc
{
    public class WellDeviation: BaseEntity
    {
        public virtual int AreaRegion { get;}
        public virtual int GeodeticDatum { get;}
        public virtual int CoordinateReferenceSystem { get;}
        public virtual string LatitudeSurfaceLocation { get; set; }
        public virtual string LongitudeSurfaceLocation { get; set; }
        public virtual Well Well { get;}
    }
}
