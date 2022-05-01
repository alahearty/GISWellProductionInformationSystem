using Orbit.Application.Extensions;
using Orbit.Application.ProductionRate.AverageRate;
using Orbit.Application.ProductionRate.TotalRate;
using Orbit.Application.Services;
using Orbit.Application.Shared;
using Orbit.Models;
using Orbit.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Application.ProductionRate.WellRate
{
    public class WellRateCommandHandler:BaseRate
    {
        public WellRateCommandHandler(IUnitOfWork unitOfWork, ProductionDataService dataService,ProductionDataPath dataPath) : base(unitOfWork, dataService,dataPath) { }
        public async Task<WellRateDTO> GetWellTotalProductionRate(WellRateCommand command)
        {
          return  await Task.Run(() =>
            {
                var wellProd = new WellRateDTO();

                if (command == null || command.WellId == Guid.Empty) return wellProd;

                var wells = _unitOfWork.WellRepository.FetchAllWells();
                var well = wells.FirstOrDefault(x => x.Id == command.WellId);

                if (well == null) return wellProd;
                var dpNames = well.DrainagePoints.Select(x => x.Name);

                var data = _dataService.GetProductionRate(_unitOfWork, _dataPath);

                var dates = data.Where(x => x.Value.Date != null && dpNames.Contains(x.Value.DrainagePointName))
                               .Select(x => x.Value.Date)
                               .Distinct();

                if (dates.NotAny()) return null;

                var lastDate = dates.Max();


                //
                var totalProdOil = new TotalRateData();
                var avgProdOil = new AverageRateData();
                var totalProdGas = new TotalRateData();
                var avgProdGas = new AverageRateData();
                var totalProdWater = new TotalRateData();
                var avgProdWater = new AverageRateData();
                var totalProdCond = new TotalRateData();
                var avgProdCond = new AverageRateData();


                var currentOilDate = GetPreviousDate(data, lastDate, dpNames, ProductionDataVariablesEnum.Oil);
                var currentGasDate = GetPreviousDate(data, lastDate, dpNames, ProductionDataVariablesEnum.Gas);
                var currentWaterDate = GetPreviousDate(data, lastDate, dpNames, ProductionDataVariablesEnum.Water);
                var currentCondDate = GetPreviousDate(data, lastDate, dpNames, ProductionDataVariablesEnum.Condensate);

                totalProdOil.CurrentDate = currentOilDate;
                totalProdGas.CurrentDate = currentGasDate;
                totalProdWater.CurrentDate = currentWaterDate;
                totalProdCond.CurrentDate = currentCondDate;

                var currentOilRates = data[currentOilDate];
                var currentGasRates = data[currentGasDate];
                var currentWaterRates = data[currentWaterDate];
                var currentCondRates = data[currentCondDate];



                ComputeCurrentProductionRate(well.Name, dpNames, totalProdOil, avgProdOil, currentOilRates, currentOilDate);
                ComputeCurrentProductionRate(well.Name, dpNames, totalProdGas, avgProdGas, currentGasRates, currentGasDate);
                ComputeCurrentProductionRate(well.Name, dpNames, totalProdWater, avgProdWater, currentWaterRates, currentWaterDate);
                ComputeCurrentProductionRate(well.Name, dpNames, totalProdCond, avgProdCond, currentCondRates, currentCondDate);


                totalProdOil.PreviousOilDate = GetPreviousDate(data, currentOilDate, dpNames, ProductionDataVariablesEnum.Oil);
                totalProdGas.PreviousGasDate = GetPreviousDate(data, currentGasDate, dpNames, ProductionDataVariablesEnum.Gas);
                totalProdWater.PreviousWaterDate = GetPreviousDate(data, currentWaterDate, dpNames, ProductionDataVariablesEnum.Water);
                totalProdCond.PreviousCondDate = GetPreviousDate(data, currentCondDate, dpNames, ProductionDataVariablesEnum.Condensate);

                var prevOilRates = data[totalProdOil.PreviousOilDate];
                var prevGasRates = data[totalProdGas.PreviousGasDate];
                var prevWaterRates = data[totalProdWater.PreviousWaterDate];
                var prevCondRates = data[totalProdCond.PreviousCondDate];

                ComputePreviousOilRate(dpNames, totalProdOil, avgProdOil, prevOilRates);
                ComputePreviousGasRate(dpNames, totalProdGas, avgProdGas, prevGasRates);
                ComputePreviousWaterRate(dpNames, totalProdWater, avgProdWater, prevWaterRates);
                ComputePreviousConsdensateRate(dpNames, totalProdCond, avgProdCond, prevCondRates);


                var percentageIncreaseInOilRate = ComputePercentageIncrease(totalProdOil.CurrentOilRate, totalProdOil.PreviousOilRate);
                var percentageIncreaseInGasRate = ComputePercentageIncrease(totalProdGas.CurrentGasRate, totalProdGas.PreviousGasRate);
                var percentageIncreaseInWaterRate = ComputePercentageIncrease(totalProdWater.CurrentWaterRate, totalProdWater.PreviousWaterRate);
                var percentageIncreaseInCondensateRate = ComputePercentageIncrease(totalProdCond.CurrentCondensateRate, totalProdCond.PreviousCondensateRate);


                wellProd.AssetName = well.Name;
                wellProd.CurrentOilRate = totalProdOil.CurrentOilRate;
                wellProd.PreviousOilRate = totalProdOil.PreviousOilRate;
                wellProd.PreviousOilDate = totalProdOil.PreviousOilDate;
                wellProd.CurrentOilDate  = totalProdOil.CurrentDate;
                wellProd.PercentageIncreaseInOilRate = percentageIncreaseInOilRate;
                wellProd.CurrentGasRate = totalProdGas.CurrentGasRate;
                wellProd.PreviousGasRate = totalProdGas.PreviousGasRate;
                wellProd.PreviousGasDate = totalProdGas.PreviousGasDate;
                wellProd.CurrentGasDate = totalProdGas.CurrentDate;
                wellProd.PercentageIncreaseInGasRate = percentageIncreaseInGasRate;
                wellProd.CurrentWaterRate = totalProdWater.CurrentWaterRate;
                wellProd.PreviousWaterRate = totalProdWater.PreviousWaterRate;
                wellProd.PreviousWaterDate = totalProdWater.PreviousWaterDate;
                wellProd.CurrentWaterDate = totalProdWater.CurrentDate;
                wellProd.PercentageIncreaseInWaterRate = percentageIncreaseInWaterRate;
                wellProd.CurrentCondensateRate = totalProdCond.CurrentCondensateRate;
                wellProd.PreviousCondensateRate = totalProdCond.PreviousCondensateRate;
                wellProd.PreviousCondDate = totalProdCond.PreviousCondDate;
                wellProd.CurrentCondDate = totalProdCond.CurrentDate;
                wellProd.PercentageIncreaseInCondensateRate = percentageIncreaseInCondensateRate;

                return wellProd;
            });
        }  

         public async Task<IEnumerable<WellRateDTO>> GetWellTotalProductionRateByDrainagePoint(WellRateCommand command)
        {
            return await Task.Run(() =>
            {
                var wellProds = new List<WellRateDTO>();

                if (command == null || command.WellId == Guid.Empty) return wellProds;

                var wells = _unitOfWork.WellRepository.FetchAllWells();
                var well = wells.FirstOrDefault(x => x.Id == command.WellId);

                if (well == null) return wellProds;
                var dpNames = well.DrainagePoints.Select(x => x.Name);

                var data = _dataService.GetProductionRate(_unitOfWork, _dataPath);

                var dates = data.Where(x => x.Value.Date != null && dpNames.Contains(x.Value.DrainagePointName))
                               .Select(x => x.Value.Date)
                               .Distinct();

                if (dates.NotAny()) return null;

                var lastDate = dates.Max();

                foreach (var dp in dpNames)
                {
                    var wellProd = new WellRateDTO();

                    var totalProdOil = new TotalRateData();
                    var avgProdOil = new AverageRateData();
                    var totalProdGas = new TotalRateData();
                    var avgProdGas = new AverageRateData();
                    var totalProdWater = new TotalRateData();
                    var avgProdWater = new AverageRateData();
                    var totalProdCond = new TotalRateData();
                    var avgProdCond = new AverageRateData();


                    var currentOilDate = GetPreviousDate(data, lastDate, new List<string> { dp }, ProductionDataVariablesEnum.Oil);
                    var currentGasDate = GetPreviousDate(data, lastDate, new List<string> { dp }, ProductionDataVariablesEnum.Gas);
                    var currentWaterDate = GetPreviousDate(data, lastDate, new List<string> { dp }, ProductionDataVariablesEnum.Water);
                    var currentCondDate = GetPreviousDate(data, lastDate, new List<string> { dp }, ProductionDataVariablesEnum.Condensate);

                    totalProdOil.CurrentDate = currentOilDate;
                    totalProdGas.CurrentDate = currentGasDate;
                    totalProdWater.CurrentDate = currentWaterDate;
                    totalProdCond.CurrentDate = currentCondDate;

                    var currentOilRates = data[currentOilDate];
                    var currentGasRates = data[currentGasDate];
                    var currentWaterRates = data[currentWaterDate];
                    var currentCondRates = data[currentCondDate];



                    ComputeCurrentProductionRate(dp, new List<string> { dp }, totalProdOil, avgProdOil, currentOilRates, currentOilDate);
                    ComputeCurrentProductionRate(dp, new List<string> { dp }, totalProdGas, avgProdGas, currentGasRates, currentGasDate);
                    ComputeCurrentProductionRate(dp, new List<string> { dp }, totalProdWater, avgProdWater, currentWaterRates, currentWaterDate);
                    ComputeCurrentProductionRate(dp, new List<string> { dp }, totalProdCond, avgProdCond, currentCondRates, currentCondDate);


                    totalProdOil.PreviousOilDate = GetPreviousDate(data, currentOilDate, new List<string> { dp }, ProductionDataVariablesEnum.Oil);
                    totalProdGas.PreviousGasDate = GetPreviousDate(data, currentGasDate, new List<string> { dp }, ProductionDataVariablesEnum.Gas);
                    totalProdWater.PreviousWaterDate = GetPreviousDate(data, currentWaterDate, new List<string> { dp }, ProductionDataVariablesEnum.Water);
                    totalProdCond.PreviousCondDate = GetPreviousDate(data, currentCondDate, new List<string> { dp }, ProductionDataVariablesEnum.Condensate);

                    var prevOilRates = data[totalProdOil.PreviousOilDate];
                    var prevGasRates = data[totalProdGas.PreviousGasDate];
                    var prevWaterRates = data[totalProdWater.PreviousWaterDate];
                    var prevCondRates = data[totalProdCond.PreviousCondDate];

                    ComputePreviousOilRate(new List<string> { dp }, totalProdOil, avgProdOil, prevOilRates);
                    ComputePreviousGasRate(new List<string> { dp }, totalProdGas, avgProdGas, prevGasRates);
                    ComputePreviousWaterRate(new List<string> { dp }, totalProdWater, avgProdWater, prevWaterRates);
                    ComputePreviousConsdensateRate(new List<string> { dp }, totalProdCond, avgProdCond, prevCondRates);


                    var percentageIncreaseInOilRate = ComputePercentageIncrease(totalProdOil.CurrentOilRate, totalProdOil.PreviousOilRate);
                    var percentageIncreaseInGasRate = ComputePercentageIncrease(totalProdGas.CurrentGasRate, totalProdGas.PreviousGasRate);
                    var percentageIncreaseInWaterRate = ComputePercentageIncrease(totalProdWater.CurrentWaterRate, totalProdWater.PreviousWaterRate);
                    var percentageIncreaseInCondensateRate = ComputePercentageIncrease(totalProdCond.CurrentCondensateRate, totalProdCond.PreviousCondensateRate);


                    wellProd.AssetName = dp;
                    wellProd.CurrentOilRate = totalProdOil.CurrentOilRate;
                    wellProd.PreviousOilRate = totalProdOil.PreviousOilRate;
                    wellProd.PreviousOilDate = totalProdOil.PreviousOilDate;
                    wellProd.CurrentOilDate = totalProdOil.CurrentDate;
                    wellProd.PercentageIncreaseInOilRate = percentageIncreaseInOilRate;
                    wellProd.CurrentGasRate = totalProdGas.CurrentGasRate;
                    wellProd.PreviousGasRate = totalProdGas.PreviousGasRate;
                    wellProd.PreviousGasDate = totalProdGas.PreviousGasDate;
                    wellProd.CurrentGasDate = totalProdGas.CurrentDate;
                    wellProd.PercentageIncreaseInGasRate = percentageIncreaseInGasRate;
                    wellProd.CurrentWaterRate = totalProdWater.CurrentWaterRate;
                    wellProd.PreviousWaterRate = totalProdWater.PreviousWaterRate;
                    wellProd.PreviousWaterDate = totalProdWater.PreviousWaterDate;
                    wellProd.CurrentWaterDate = totalProdWater.CurrentDate;
                    wellProd.PercentageIncreaseInWaterRate = percentageIncreaseInWaterRate;
                    wellProd.CurrentCondensateRate = totalProdCond.CurrentCondensateRate;
                    wellProd.PreviousCondensateRate = totalProdCond.PreviousCondensateRate;
                    wellProd.PreviousCondDate = totalProdCond.PreviousCondDate;
                    wellProd.CurrentCondDate = totalProdCond.CurrentDate;
                    wellProd.PercentageIncreaseInCondensateRate = percentageIncreaseInCondensateRate;

                    wellProds.Add(wellProd);
                }

               

                return wellProds;
            });
        }
    }
}
