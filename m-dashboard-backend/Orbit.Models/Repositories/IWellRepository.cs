using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Models.Repositories
{
    public interface IWellRepository
    {
        IList<Assets.Well> FetchAllWells();
        Assets.Well FindWellByName(string name);
        Assets.Well FindWellById(Guid id);
    }
}
