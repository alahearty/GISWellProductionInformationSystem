using Orbit.Models.Adhoc;
using System.Collections.Generic;

namespace Orbit.Models.Repositories
{
    public interface IVariableMappingRepository
    {
        IList<VariableMapping> FetchAllVariableMappings();
    }
}
