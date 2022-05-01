using NHibernate;
using Orbit.Models.Adhoc;
using Orbit.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.NHibernate.Repositories
{
    public class WorkSheetRepository : Repository, IWorkSheetRepository
    {
        public WorkSheetRepository(ISession session) : base(session) { }
        public IList<WorkSheet> FetchAllWorkSheets()
        {
            return _session.QueryOver<WorkSheet>().List();
        }
    }
}
