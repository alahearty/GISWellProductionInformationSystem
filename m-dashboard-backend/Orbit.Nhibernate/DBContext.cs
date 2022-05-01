using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using Orbit.Models;
using Orbit.NHibernate.Mappings;

namespace Orbit.NHibernate
{
    public class DBContext
    {
        public DBContext(string connectionString)
        {
            if (SessionFactory == null)
                SessionFactory = BuildSessionFactory_SQLite(connectionString);
        }

        public ISession OpenSession() => SessionFactory.OpenSession();

        public ISessionFactory SessionFactory { get; }

        private ISessionFactory BuildSessionFactory_SQLServer(string connectionString)
        {
            FluentConfiguration configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings.AddFromAssembly(typeof(BaseMap<BaseEntity>).Assembly))
                .ExposeConfiguration(cfg =>
                {
                    //cfg.SetInterceptor(new SqlStatementInterceptor());
                    new SchemaUpdate(cfg).Execute(true, true);
                    cfg.SetProperty(Environment.CommandTimeout, "2000");
                });

            return configuration.BuildSessionFactory();
        }

        private ISessionFactory BuildSessionFactory_SQLite(string connectionString)
        {
            FluentConfiguration configuration = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings
                    .AddFromAssembly(typeof(BaseMap<BaseEntity>).Assembly))
                    .ExposeConfiguration(cfg => {
                        new SchemaUpdate(cfg).Execute(false, true);
                        cfg.SetProperty(Environment.CommandTimeout, "2000");
                    });
            return configuration.BuildSessionFactory();
        }

    }
}
