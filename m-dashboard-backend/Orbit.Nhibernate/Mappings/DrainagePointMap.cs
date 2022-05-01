using Orbit.Models.Assets;
using Orbit.NHibernate.Mappings;

namespace Orbit.Nhibernate.Mappings
{
    public class DrainagePointMap : BaseMap<DrainagePoint>
    {
        public DrainagePointMap()
        {
            Table("WSE_DrainagePoint");
            LazyLoad();
            References(x => x.Well).Not.LazyLoad();
            References(x => x.Reservoir).Not.LazyLoad();
            Map(x => x.StringCode);
        }

    }
}
