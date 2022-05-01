using Orbit.Models.Units;
using Orbit.NHibernate.Mappings;

namespace Orbit.Nhibernate.Mappings
{
    public class UnitPropertyMap : BaseMap<UnitProperty>
    {
        public UnitPropertyMap()
        {
            Table("CF_EngineeringProperty");
            LazyLoad();
            References(x => x.DatabaseUnit);
            References(x => x.DefaultUnit);
            References(x => x.DatabaseUnitMultiplier);
            References(x => x.DefaultUnitMultiplier);
            Map(x => x.Name).Not.Nullable().Column("PropertyName");
            Map(x => x.Category).Not.Nullable();
        }
    }
}
