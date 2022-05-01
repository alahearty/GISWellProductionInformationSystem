using NHibernate;
using Orbit.Models.Assets;
using Orbit.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orbit.NHibernate.Repositories
{
    public class FieldRepository:Repository,IFieldRepository
    {
        public FieldRepository(ISession session):base(session)  {  }

        public IList<Field> FetchAllFields()
        {
            return _session.QueryOver<Field>().List();
        }

        public Field FindFieldById(Guid id)
        {
            return _session.QueryOver<Field>().List().FirstOrDefault(x=>x.Id==id);
        }

        public Field FindFieldByName(string name)
        {
            return _session.QueryOver<Field>().List().FirstOrDefault(x => x.Name == name);
        }
    }
}
