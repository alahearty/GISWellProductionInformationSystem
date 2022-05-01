using Orbit.Models.Adhoc;
using Orbit.NHibernate.Mappings;

namespace Orbit.Nhibernate.Mappings
{
    public class WorkSheetMap : BaseMap<WorkSheet>
    {
        public WorkSheetMap()
        {
            Table("DI_UserTables");
            LazyLoad();
            References(x => x.Type);
            Map(x => x.Name).Not.Nullable().Column("Title"); 
            HasMany(x => x.Columns).Inverse().Cascade.All();
        }
    }
}
