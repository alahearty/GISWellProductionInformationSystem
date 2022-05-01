using System;
using System.Collections.Generic;

namespace Orbit.Models.Repositories
{
    public interface IDrainagePointRepository
    {
        IList<Assets.DrainagePoint> FetchAllDrainagepoints();
        Assets.DrainagePoint FindDrainagePointByName(string name);
        Assets.DrainagePoint FindDrainagePointById(Guid id);
    }
}
