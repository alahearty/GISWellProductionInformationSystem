using Orbit.Models.Units;
using Orbit.NHibernate.Mappings;

namespace Orbit.Nhibernate.Mappings
{

    public class QuantityMap : BaseMap<Quantity>
    {
        public QuantityMap()
        {
            Table("CF_Quantity");
            LazyLoad();
            Map(x => x.Name);
            HasMany(x => x.Units).Cascade.All();
            Join("CF_Quantity_BaseUnit", x =>
            {
                x.KeyColumn("Quantity_id");
                x.References(i => i.BaseUnit).ForeignKey("Unit_id").Not.LazyLoad();
            });
        }
    }
}
