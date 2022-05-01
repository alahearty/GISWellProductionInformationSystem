using NHibernate;

namespace Orbit.NHibernate.Repositories
{
    public abstract class Repository
    {
        public Repository(ISession session) => _session = session;
        protected ISession _session;
    }
    
}
