namespace Orbit.Models.Repositories
{
    public interface IUnitOfWork
    {
        IReservoirRepository ReservoirRepository { get;}
        IWellRepository WellRepository { get;}
        IFieldRepository FieldRepository { get;}
        IDrainagePointRepository DrainagePointRepository { get;}
        IWorkSheetRepository WorkSheetRepository { get;}
        IUnitPropertyRepository UnitPropertyRepository { get;}
        IVariableMappingRepository VariableMappingRepository { get;}
    }
}
