using Orbit.NHibernate.Mappings;
using Orbit.Models.Units;

namespace Orbit.Nhibernate.Mappings
{

    public class MultiplierMap : BaseMap<Multiplier>
    {
        public MultiplierMap()
        {
            Table("CF_Multiplier");
            LazyLoad();
            Map(x => x.MultiplierSymbol);
            Map(x => x.Multiplicand);
            References(x => x.Unit);
        }
    }
}
