using NHibernate;
using Orbit.Models.Assets;
using Orbit.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orbit.NHibernate.Repositories
{
    public class ReservoirRepository:Repository,IReservoirRepository
    {
        public ReservoirRepository(ISession session) : base(session) { }

        public IList<Reservoir> FetchAllReservoirs()
        {
            return _session.QueryOver<Reservoir>().List();
        }

        public Reservoir FindReservoirById(Guid id)
        {
            return _session.QueryOver<Reservoir>().List().FirstOrDefault(x => x.Id == id);
        }

        public Reservoir FindReservoirByName(string name)
        {
            return _session.QueryOver<Reservoir>().List().FirstOrDefault(x => x.Name == name);
        }
    }
}
