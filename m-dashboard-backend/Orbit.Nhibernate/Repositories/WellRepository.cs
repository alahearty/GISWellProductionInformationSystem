using NHibernate;
using Orbit.Models.Assets;
using Orbit.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orbit.NHibernate.Repositories
{
    public class WellRepository : Repository, IWellRepository
    {
        public WellRepository(ISession session) : base(session) { }
        public IList<Well> FetchAllWells()
        {
            return _session.QueryOver<Well>().List();
        }

        public Well FindWellById(Guid id)
        {
            return _session.QueryOver<Well>().List().FirstOrDefault(x => x.Id == id);
        }

        public Well FindWellByName(string name)
        {
            return _session.QueryOver<Well>().List().FirstOrDefault(x => x.Name == name);
        }

        
    }
}
