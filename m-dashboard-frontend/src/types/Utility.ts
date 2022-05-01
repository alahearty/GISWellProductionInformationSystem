import { Rate } from "./TotalProduction"

export type FormSelect<T = string> = {
    value: T;
    text: string;
}

export type RequestObject = {
    Ids: string[];
    AssetType: number;
}

export type ProductionStatistics = {
    CurrentOil: string;
    PreviousOil: string;
    AverageOil: string;

    PercentageOilGain: string;
    PercentageGasGain: string;

    CurrentGas: string;
    PreviousGas: string;
    AverageGas: string;

    AverageWater: string;
    WellCount: number;
}

export type WellRate = {
    CurrentOilRate: string;
    PreviousOilRate: string;
    CurrentOilDate: string;
    PreviousOilDate: string;

    CurrentGasRate: string;
    PreviousGasRate: string;
    CurrentGasDate: string;
    PreviousGasDate: string;

    CurrentWaterRate: string;
    PreviousWaterRate: string;
    CurrentWaterDate: string;
    PreviousWaterDate: string;

    CurrentCondensateRate: string;
    PreviousCondensateRate: string;
    CurrentCondDate: string;
    PreviousCondDate: string;

    PercentageIncreaseInCondensateRate: string;
    PercentageIncreaseInOilRate: string;
    PercentageIncreaseInGasRate: string;
    PercentageIncreaseInWaterRate: string;
}

export type AvgResponse = {
    Name: string;
    Unit: string;
    Value: number;
}

export type DataSet = {
    data: number[];
    label?: string;
    backgroundColor?: string | string[] | CanvasPattern;
    steppedLine?: boolean;
    borderWidth?: number;
    yAxisID?: string;
    spanGroups?: boolean;
    pointRadius?: number;
    lineTension?: 0;
    datalabels?: {color: string};
    type?: string;
    borderColor?: string | string[];
    fill?: boolean;
}

export type ChartData = {
    labels?: string[] | string[];
    datasets: DataSet[];
}

export type PerformanceRanker = {
    AssetName: string;
    Unit: string;
    OilValue: number;
    GasValue: number;
    WaterValue: number;
}

export type PerformancePlotData = {
    Date: string;
    OilRate: number;
    GOR: number;
    WaterCut: number;
    CumOil: number;
}

export type AverageProduction = {
    // AssetName: string;
    // AverageMonthlyOil: number;
    // AverageMonthlyGas: number;
    // AverageMonthlyWater: number;
    // OilValue: number;
    // GasValue: number;
    // WaterValue: number;
    AverageRates: Rate[]
    PercentageIncreaseInCondensateRate: string;
    PercentageIncreaseInOilRate: string;
    PercentageIncreaseInGasRate: string;
    PercentageIncreaseInWaterRate: string;
}

export type AverageProductionRequest = {
    Ids: string[];
    AssetType: number;
}

export interface RequestByDate extends AverageProductionRequest {
    Date: string;
}

export type PerformancePlot = {
    Data: PerformancePlotData[];
    GasUnit: string;
    OilUnit: string;
    WaterUnit: string;
}

export type Asset = {
    Id: string;
    AssetType?: number;
    DisplayName?: string;
}

export type AssetExplorer = {
    Assets: {
        OML: Asset[];
        Field: Asset[];
        Reservoir: Asset[];
        Well: Asset[];
        None: Asset[];
    }
}

export type Explorer = {
    DisplayName: string;
    Id: string;
    Fields: Field[];
}

export type Well = {
    Longitude: number;
    Latitude: number;
    Id: string;
    WellId: string;
    WellName: string;
    DisplayName: string;
}

export type Reservoir = {
    Id: string;
    DisplayName: string;
    DrainagePoints: Well[];
}

export type Field = {
    Wells: Well[];
    Reservoirs?: Reservoir[];
    Id: string;
    DisplayName: string;
}