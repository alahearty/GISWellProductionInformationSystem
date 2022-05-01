using Orbit.Models.Adhoc;
using Orbit.NHibernate.Mappings;

namespace Orbit.Nhibernate.Mappings
{
    public class WorkSheetTypeMap : BaseMap<WorkSheetType>
    {
        public WorkSheetTypeMap()
        {
            Table("DI_TableTypes");
            LazyLoad();
            Map(x => x.Name).Not.Nullable().Column("Title"); 
            Map(x => x.TypeName).CustomType<SheetType>().Not.Nullable();
            HasMany(x => x.Variables).Inverse().Cascade.All().LazyLoad();
            HasMany(x => x.WorkSheets).Inverse().Cascade.All().LazyLoad();
        }
    }
}
