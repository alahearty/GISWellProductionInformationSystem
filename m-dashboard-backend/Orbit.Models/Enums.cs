namespace Orbit.Models
{
    public enum AssetType {OML, Field, Reservoir,Well, DrainagePoint, None }

    public enum ReservoirTypeEnum
    {
        OilGas,
        Gas,
        Gas_Condensate,
        Hydrocarbon
    }
    public enum ProductionDataVariablesEnum
    {
        DrainagePoint,
        ProductionDate,
        Oil,
        Gas,
        Water,
        Bean,
        THP,
        Sand,
        ProdDays,
        WaterInjection,
        GasInjection,
        Condensate,
        WaterCut

    }

    public enum BHPVariablesEnum
    {
        DrainagePoint,
        Date,
        DatumPressure,
        Source
    }

    public enum WellTestEnum
    {
        DrainagePoint,
        Date,
        BeanSize,
        GrossLiquidRate,
        NetOilRate,
        GasRate,
        GOR,
        CGR,
        WGR,
        BSW,
        SandProduced,
        FTHP,
        CasingHeadPressure
    }

    public enum AdhocVariablesEnum
    {
        WellHeadRating,
        WellLocation,
        FluidType
    }
}
