export default class TotalProduction {
    // TotalOilProduction: Record<string, number>;
    // TotalGasProduction: Record<string, number>;
    // TotalWaterProduction: Record<string, number>;
    // TotalOilRate: Record<string, number>;
    // TotalGasRate: Record<string, number>;
    // TotalWaterRate: Record<string, number>;
    Dates: string[];
    TotalRates: Rate[];
    PercentageIncreaseInCondensateRate: string;
    PercentageIncreaseInOilRate: string;
    PercentageIncreaseInGasRate: string;
    PercentageIncreaseInWaterRate: string;

    constructor() {
        // this.TotalOilProduction = {};
        // this.TotalGasProduction = {};
        // this.TotalWaterProduction = {};
        // this.TotalOilRate = {};
        // this.TotalGasRate = {};
        // this.TotalWaterRate = {};
        this.Dates = [];
        this.TotalRates = [];
        this.PercentageIncreaseInCondensateRate = '';
        this.PercentageIncreaseInOilRate = '';
        this.PercentageIncreaseInGasRate = '';
        this.PercentageIncreaseInWaterRate = '';
    }
}

export type Rate = {
    AssetName: string;
    CurrentCondensateRate: number;
    CurrentGasRate: number;
    CurrentGasRateBOE: number;
    CurrentOilRate: number;
    CurrentWaterRate: number;
    CurrentDate: string;
    PreviousCondensateRate: number;
    PreviousOilRate: number;
    PreviousGasRate: number;
    PreviousWaterRate: number;
    PreviousOilDate: string;
    PreviousGasDate: string;
    PreviousWaterDate: string;
    PreviousCondDate: string;
    PercentageIncreaseInCondensateRate: string;
    PercentageIncreaseInOilRate: string;
    PercentageIncreaseInGasRate: string;
    PercentageIncreaseInWaterRate: string;
}