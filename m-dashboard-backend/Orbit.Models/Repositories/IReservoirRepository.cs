using System;
using System.Collections.Generic;

namespace Orbit.Models.Repositories
{
    public interface IReservoirRepository
    {
        IList<Assets.Reservoir> FetchAllReservoirs();
        Assets.Reservoir FindReservoirByName(string name);
        Assets.Reservoir FindReservoirById(Guid id);
    }
}
