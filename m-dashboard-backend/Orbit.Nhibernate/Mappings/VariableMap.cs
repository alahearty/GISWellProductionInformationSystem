using Orbit.Models.Adhoc;
using Orbit.NHibernate.Mappings;

namespace Orbit.Nhibernate.Mappings
{
    public class VariableProxyMap : BaseMap<Variable>
    {
        public VariableProxyMap()
        {
            Table("DI_Variables");
            LazyLoad();

            Map(x => x.Name).Not.Nullable();
            Map(x => x.DataType).CustomType<VariableDataType>().Not.Nullable();
            References(x => x.TableType);
            References(x => x.DefaultUnit);
            References(x => x.DefaultMultiplier).Nullable();
        }
    }
}
