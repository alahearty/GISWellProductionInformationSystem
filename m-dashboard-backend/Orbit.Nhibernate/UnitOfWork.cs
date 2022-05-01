using Orbit.Models.Repositories;
using Orbit.NHibernate.Repositories;

namespace Orbit.NHibernate
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(DBContext dB)
        {
            var session = dB.OpenSession();
            FieldRepository = new FieldRepository(session);
            WellRepository = new WellRepository(session);
            ReservoirRepository = new ReservoirRepository(session);
            WorkSheetRepository = new WorkSheetRepository(session);
            VariableMappingRepository = new VariableMappingRepository(session);
            DrainagePointRepository = new DrainagePointRepository(session);
            UnitPropertyRepository = new UnitPropertyRepository(session);
        }
        public IReservoirRepository ReservoirRepository { get; }
        public IWellRepository WellRepository { get; }
        public IFieldRepository FieldRepository { get; }
        public IDrainagePointRepository DrainagePointRepository { get; }
        public IWorkSheetRepository WorkSheetRepository { get; }
        public IUnitPropertyRepository UnitPropertyRepository { get; }
        public IVariableMappingRepository VariableMappingRepository { get; }
    }
}
