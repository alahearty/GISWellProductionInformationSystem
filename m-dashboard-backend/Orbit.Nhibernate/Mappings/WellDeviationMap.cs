using Orbit.Models.Adhoc;
using Orbit.NHibernate.Mappings;

namespace Orbit.Nhibernate.Mappings
{
    public class WellDeviationMap: BaseMap<WellDeviation>
    {
        public WellDeviationMap()
        {
            Table("WSE_WellDeviation");
            LazyLoad();

            References(x => x.Well);
            Map(x => x.AreaRegion);
            Map(x => x.GeodeticDatum);
            Map(x => x.CoordinateReferenceSystem);
            Map(x => x.LatitudeSurfaceLocation);
            Map(x => x.LongitudeSurfaceLocation);
        }
    }
}
