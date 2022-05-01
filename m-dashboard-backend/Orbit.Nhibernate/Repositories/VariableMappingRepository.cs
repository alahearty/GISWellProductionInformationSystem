using NHibernate;
using Orbit.Models.Adhoc;
using Orbit.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.NHibernate.Repositories
{
    public class VariableMappingRepository : Repository, IVariableMappingRepository
    {
        public VariableMappingRepository(ISession session) : base(session) { }
        public IList<VariableMapping> FetchAllVariableMappings()
        {
            return _session.QueryOver<VariableMapping>().List();
        }
    }
}
