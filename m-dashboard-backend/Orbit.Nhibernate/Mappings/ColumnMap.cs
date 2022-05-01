using Orbit.Models.Adhoc;
using Orbit.NHibernate.Mappings;

namespace Orbit.Nhibernate.Mappings
{
    public class ColumnMap : BaseMap<Column>
    {
        public ColumnMap()
        {
            Table("DI_UserTableColumns");
            LazyLoad();
            Map(x => x.Name);
            References(x => x.Unit);
            References(x => x.Multiplier);
            References(x => x.WorkSheet).Column("Table_id");
            HasMany(x => x.ColumnMapping).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}