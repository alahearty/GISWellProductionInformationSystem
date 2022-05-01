using NHibernate;
using Orbit.Models.Repositories;
using Orbit.Models.Units;
using System.Collections.Generic;

namespace Orbit.NHibernate.Repositories
{
    public class UnitPropertyRepository : Repository, IUnitPropertyRepository
    {
        public UnitPropertyRepository(ISession session) : base(session) { }
        public IList<UnitProperty> FetchAllUniProperties()
        {
            return _session.QueryOver<UnitProperty>().List();
        }
    }
}
