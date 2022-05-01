using NHibernate;
using Orbit.Models.Assets;
using Orbit.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orbit.NHibernate.Repositories
{
    public class DrainagePointRepository : Repository, IDrainagePointRepository
    {
        public DrainagePointRepository(ISession session) : base(session) { }
        public IList<DrainagePoint> FetchAllDrainagepoints()
        {
            return _session.QueryOver<DrainagePoint>().List();
        }

        public DrainagePoint FindDrainagePointById(Guid id)
        {
            return _session.QueryOver<DrainagePoint>().List().FirstOrDefault(x => x.Id == id);
        }

        public DrainagePoint FindDrainagePointByName(string name)
        {
            return _session.QueryOver<DrainagePoint>().List().FirstOrDefault(x => x.Name == name);
        }
    }
}
