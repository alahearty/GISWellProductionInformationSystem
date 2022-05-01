using System;
using System.Collections.Generic;

namespace Orbit.Models.Repositories
{
    public interface IFieldRepository
    {
        IList<Assets.Field> FetchAllFields();
        Assets.Field FindFieldByName(string name);
        Assets.Field FindFieldById(Guid id);
    }
}
