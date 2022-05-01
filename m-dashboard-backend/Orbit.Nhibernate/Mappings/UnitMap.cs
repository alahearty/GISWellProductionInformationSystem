using Orbit.Models.Units;
using Orbit.NHibernate.Mappings;

namespace Orbit.Nhibernate.Mappings
{
    public class UnitMap : BaseMap<Unit>
    {
        public UnitMap()
        {
            Table("CF_Unit");
            LazyLoad();
            Map(x => x.Symbol);
            Map(x => x.Multiplicand);
            Map(x => x.Addend);
            References(x => x.Quantity);
            HasMany(x => x.Multipliers).Inverse().Cascade.All();
        }
    }
}
