

using Orbit.Models.Adhoc;
using Orbit.NHibernate.Mappings;

namespace Orbit.Nhibernate.Mappings
{
    public class VariableMappingMap : BaseMap<VariableMapping>
    {
        public VariableMappingMap()
        {
            Table("DI_VariableMappings");
            LazyLoad();
            References(x => x.Variable);
            References(x => x.TargetColumn);
        }
    }
}
