using Orbit.Models.Assets;

namespace Orbit.NHibernate.Mappings
{
    public class WellMap : BaseMap<Well>
    {
        public WellMap()
        {
            Table("WSE_Well");
            LazyLoad();
            Map(x => x.WellCode).Not.Nullable();
            Map(x => x.SurfaceCoordinateX).Not.Nullable();
            Map(x => x.SurfaceCoordinateY).Not.Nullable();
            References(x => x.Field);
            HasMany(x => x.DrainagePoints).Inverse().Cascade.All().Not.LazyLoad();
            HasMany(x => x.WellDeviations).Inverse().Cascade.All();

        }
    }
}
