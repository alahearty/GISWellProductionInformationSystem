using Orbit.Models;
using Orbit.Models.Assets;
using Orbit.NHibernate.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Nhibernate.Mappings
{
    public class ReservoirMap : BaseMap<Reservoir>
    {
        public ReservoirMap()
        {
            Table("WSE_Reservoir");
            LazyLoad();
            References(x => x.Field);
            Map(x => x.Name).Not.Nullable();
            Map(x => x.ReservoirType).CustomType<ReservoirTypeEnum>().Not.Nullable();
            HasMany(x => x.DrainagePoints).Inverse().Cascade.All();

        }
    }
}
