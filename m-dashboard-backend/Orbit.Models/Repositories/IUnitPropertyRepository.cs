using Orbit.Models.Units;
using System.Collections.Generic;

namespace Orbit.Models.Repositories
{
    public interface IUnitPropertyRepository
    {
        IList<UnitProperty> FetchAllUniProperties();
    }
}
