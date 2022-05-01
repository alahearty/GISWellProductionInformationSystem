using Orbit.Models.Assets;
using Orbit.NHibernate.Mappings;

namespace Orbit.Nhibernate.Mappings
{
    public class FieldMap : BaseMap<Field>
    {
        public FieldMap()
        {
            Table("WSE_Field");
            LazyLoad();
            Map(x => x.Name).Not.Nullable();
            Map(x => x.OML);
            HasMany(x => x.Reservoirs).Inverse().Cascade.All().LazyLoad();
            HasMany(x => x.Wells).Inverse().Cascade.All().LazyLoad();
        }

    }
}
