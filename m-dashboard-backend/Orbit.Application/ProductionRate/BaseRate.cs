using Orbit.Application.Extensions;
using Orbit.Application.ProductionRate.AverageRate;
using Orbit.Application.ProductionRate.TotalRate;
using Orbit.Application.Services;
using Orbit.Application.Shared;
using Orbit.Models;
using Orbit.Models.Assets;
using Orbit.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Application.ProductionRate
{
    public abstract class BaseRate
    {
        public BaseRate(IUnitOfWork unitOfWork, ProductionDataService dataService,ProductionDataPath dataPath)
        {
            _unitOfWork = unitOfWork;
            _dataService = dataService;
            _dataPath = dataPath;
        }
        protected async Task<TotalAvgRate> GetDefaultProductionRate()
        {
            if (defaultDotalAvgRate != null) return defaultDotalAvgRate;
            var fields = _unitOfWork.FieldRepository.FetchAllFields();
            var allOMLs = fields.Select(i => i.OML).Distinct();
            var omlFields = fields.Where(x => allOMLs.Contains(x.OML.ToString()));
            var dpNames = omlFields.SelectMany(x => x.DrainagePoints).Select(x => x.Name);

            var data = _dataService.GetProductionRate(_unitOfWork, _dataPath);

            var dates = data.Where(x => x.Value.Date != null)
                           .Select(x => x.Value.Date)
                           .OrderBy(x => x)
                           .Distinct();

            if (dates.NotAny()) return null;

            var lastDate = dates.Max();

            var currentrates = data[lastDate];

            defaultDotalAvgRate = new TotalAvgRate
            {
                Dates = dates
            };

            return await GetTotalAvgRateATOML(defaultDotalAvgRate, currentrates, data, lastDate, allOMLs, omlFields);
        }

        protected async Task<TotalAvgRate> GetProductionRateAtAsset(DateTime? date, IEnumerable<IAsset> assets)
        {
            var data = _dataService.GetProductionRate(_unitOfWork, _dataPath);

            var dates = data.Where(x => x.Value.Date != null)
                           .Select(x => x.Value.Date)
                           .OrderBy(x => x)
                           .Distinct();

            if (dates.NotAny() || date == null) return null;

            var currentrates = data[date];

            var totalAvgProd = new TotalAvgRate
            {
                Dates = dates
            };

            return await GetTotalAvgProductionRateAtAsset(totalAvgProd, currentrates, data, date, assets);
        }

        protected async Task<TotalAvgRate> GetProductionRateAtDrainagePoint(DateTime? date, IEnumerable<string> dpNames)
        {
            var data = _dataService.GetProductionRate(_unitOfWork, _dataPath);

            var dates = data.Where(x => x.Value.Date != null)
                           .Select(x => x.Value.Date)
                           .OrderBy(x => x)
                           .Distinct();

            if (dates.NotAny() || date == null) return null;

            var currentrates = data[date];

            var totalAvgProd = new TotalAvgRate
            {
                Dates = dates
            };

            return await GetTotalAvgProductionRateAtDrianagePoint(totalAvgProd, currentrates, data, date, dpNames);
        }
        protected async Task<TotalAvgRate> GetProductionRateAtOML(DateTime? date, IEnumerable<string> omls, IEnumerable<Field> fields)
        {
            var data = _dataService.GetProductionRate(_unitOfWork, _dataPath);

            var dates = data.Where(x => x.Value.Date != null)
                           .Select(x => x.Value.Date)
                           .OrderBy(x => x)
                           .Distinct();

            if (dates.NotAny() || date == null) return null;

            var currentrates = data[date];

            var totalAvgProd = new TotalAvgRate
            {
                Dates = dates
            };

            return await GetTotalAvgRateATOML(totalAvgProd, currentrates, data, date, omls, fields);
        }

        protected async Task<TotalAvgRate> GetTotalAvgRateATOML(TotalAvgRate totalAvgProd, IEnumerable<ProductionRateData> currentRates,
            PowerCollection<DateTime?, ProductionRateData> data, DateTime? date, IEnumerable<string> omls, IEnumerable<Field> fields)
        {
            return await Task.Factory.StartNew(() =>
            {
                foreach (var oml in omls)
                {
                    var dpNames = fields.Where(x => x.OML == oml)
                                    .SelectMany(x => x.DrainagePoints)
                                    .Select(y => y.Name);
                    if (dpNames.Any())
                    {
                        var result = GetData($"OML-{oml}", dpNames, currentRates, date, data);

                        totalAvgProd.TotalRates.Add(result.Item1);
                        totalAvgProd.AverageRates.Add(result.Item2);
                    }
                }
                totalAvgProd.TotalPercentageIncreaseInOilRate = ComputePercentageIncrease(totalAvgProd.TotalRates.Sum(x => x.CurrentOilRate), totalAvgProd.TotalRates.Sum(x => x.PreviousOilRate));
                totalAvgProd.TotalPercentageIncreaseInGasRate = ComputePercentageIncrease(totalAvgProd.TotalRates.Sum(x => x.CurrentGasRate), totalAvgProd.TotalRates.Sum(x => x.PreviousGasRate));
                totalAvgProd.TotalPercentageIncreaseInWaterRate = ComputePercentageIncrease(totalAvgProd.TotalRates.Sum(x => x.CurrentWaterRate), totalAvgProd.TotalRates.Sum(x => x.PreviousWaterRate));
                totalAvgProd.TotalPercentageIncreaseInCondensateRate = ComputePercentageIncrease(totalAvgProd.TotalRates.Sum(x => x.CurrentCondensateRate), totalAvgProd.TotalRates.Sum(x => x.PreviousCondensateRate));

                totalAvgProd.AvgPercentageIncreaseInOilRate = ComputePercentageIncrease(totalAvgProd.AverageRates.Sum(x => x.CurrentOilRate), totalAvgProd.AverageRates.Sum(x => x.PreviousOilRate));
                totalAvgProd.AvgPercentageIncreaseInGasRate = ComputePercentageIncrease(totalAvgProd.AverageRates.Sum(x => x.CurrentGasRate), totalAvgProd.AverageRates.Sum(x => x.PreviousGasRate));
                totalAvgProd.AvgPercentageIncreaseInWaterRate = ComputePercentageIncrease(totalAvgProd.AverageRates.Sum(x => x.CurrentWaterRate), totalAvgProd.AverageRates.Sum(x => x.PreviousWaterRate));
                totalAvgProd.AvgPercentageIncreaseInCondensateRate = ComputePercentageIncrease(totalAvgProd.AverageRates.Sum(x => x.CurrentCondensateRate), totalAvgProd.AverageRates.Sum(x => x.PreviousCondensateRate));

                return totalAvgProd;
            });
        }


        protected async Task<TotalAvgRate> GetTotalAvgProductionRateAtAsset(TotalAvgRate totalAvgProd, IEnumerable<ProductionRateData> rates,
           PowerCollection<DateTime?, ProductionRateData> data, DateTime? date, IEnumerable<IAsset> assets)
        {
            return await Task.Factory.StartNew(() =>
            {
                foreach (var asset in assets)
                {
                    var dpNames = asset.DrainagePoints.Select(x => x.Name);
                    if (dpNames.Any())
                    {
                        var result = GetData(asset.Name, dpNames, rates, date, data);

                        totalAvgProd.TotalRates.Add(result.Item1);
                        totalAvgProd.AverageRates.Add(result.Item2);
                    }
                }
                totalAvgProd.TotalPercentageIncreaseInOilRate = ComputePercentageIncrease(totalAvgProd.TotalRates.Sum(x => x.CurrentOilRate), totalAvgProd.TotalRates.Sum(x => x.PreviousOilRate));
                totalAvgProd.TotalPercentageIncreaseInGasRate = ComputePercentageIncrease(totalAvgProd.TotalRates.Sum(x => x.CurrentGasRate), totalAvgProd.TotalRates.Sum(x => x.PreviousGasRate));
                totalAvgProd.TotalPercentageIncreaseInWaterRate = ComputePercentageIncrease(totalAvgProd.TotalRates.Sum(x => x.CurrentWaterRate), totalAvgProd.TotalRates.Sum(x => x.PreviousWaterRate));
                totalAvgProd.TotalPercentageIncreaseInCondensateRate = ComputePercentageIncrease(totalAvgProd.TotalRates.Sum(x => x.CurrentCondensateRate), totalAvgProd.TotalRates.Sum(x => x.PreviousCondensateRate));

                totalAvgProd.AvgPercentageIncreaseInOilRate = ComputePercentageIncrease(totalAvgProd.AverageRates.Sum(x => x.CurrentOilRate), totalAvgProd.AverageRates.Sum(x => x.PreviousOilRate));
                totalAvgProd.AvgPercentageIncreaseInGasRate = ComputePercentageIncrease(totalAvgProd.AverageRates.Sum(x => x.CurrentGasRate), totalAvgProd.AverageRates.Sum(x => x.PreviousGasRate));
                totalAvgProd.AvgPercentageIncreaseInWaterRate = ComputePercentageIncrease(totalAvgProd.AverageRates.Sum(x => x.CurrentWaterRate), totalAvgProd.AverageRates.Sum(x => x.PreviousWaterRate));
                totalAvgProd.AvgPercentageIncreaseInCondensateRate = ComputePercentageIncrease(totalAvgProd.AverageRates.Sum(x => x.CurrentCondensateRate), totalAvgProd.AverageRates.Sum(x => x.PreviousCondensateRate));
                return totalAvgProd;
            });
        }


        protected async Task<TotalAvgRate> GetTotalAvgProductionRateAtDrianagePoint(TotalAvgRate totalAvgProd, IEnumerable<ProductionRateData> rates,
          PowerCollection<DateTime?, ProductionRateData> data, DateTime? date,
          IEnumerable<string> dpNames)
        {
            return await Task.Factory.StartNew(() =>
            {
                foreach (var dp in dpNames)
                {
                    var result = GetData(dp, new string[] { dp }, rates, date, data);

                    totalAvgProd.TotalRates.Add(result.Item1);
                    totalAvgProd.AverageRates.Add(result.Item2);
                }
                totalAvgProd.TotalPercentageIncreaseInOilRate = ComputePercentageIncrease(totalAvgProd.TotalRates.Sum(x => x.CurrentOilRate), totalAvgProd.TotalRates.Sum(x => x.PreviousOilRate));
                totalAvgProd.TotalPercentageIncreaseInGasRate = ComputePercentageIncrease(totalAvgProd.TotalRates.Sum(x => x.CurrentGasRate), totalAvgProd.TotalRates.Sum(x => x.PreviousGasRate));
                totalAvgProd.TotalPercentageIncreaseInWaterRate = ComputePercentageIncrease(totalAvgProd.TotalRates.Sum(x => x.CurrentWaterRate), totalAvgProd.TotalRates.Sum(x => x.PreviousWaterRate));
                totalAvgProd.TotalPercentageIncreaseInCondensateRate = ComputePercentageIncrease(totalAvgProd.TotalRates.Sum(x => x.CurrentCondensateRate), totalAvgProd.TotalRates.Sum(x => x.PreviousCondensateRate));

                totalAvgProd.AvgPercentageIncreaseInOilRate = ComputePercentageIncrease(totalAvgProd.AverageRates.Sum(x => x.CurrentOilRate), totalAvgProd.AverageRates.Sum(x => x.PreviousOilRate));
                totalAvgProd.AvgPercentageIncreaseInGasRate = ComputePercentageIncrease(totalAvgProd.AverageRates.Sum(x => x.CurrentGasRate), totalAvgProd.AverageRates.Sum(x => x.PreviousGasRate));
                totalAvgProd.AvgPercentageIncreaseInWaterRate = ComputePercentageIncrease(totalAvgProd.AverageRates.Sum(x => x.CurrentWaterRate), totalAvgProd.AverageRates.Sum(x => x.PreviousWaterRate));
                totalAvgProd.AvgPercentageIncreaseInCondensateRate = ComputePercentageIncrease(totalAvgProd.AverageRates.Sum(x => x.CurrentCondensateRate), totalAvgProd.AverageRates.Sum(x => x.PreviousCondensateRate));
                return totalAvgProd;
            });
        }

        protected void ComputeCurrentProductionRate(string assetName, IEnumerable<string> dpNames, TotalRateData totalProd, AverageRateData avgProd, IEnumerable<ProductionRateData> rates, DateTime? currentDate)
        {
            var oilList = rates.Where(x => dpNames.Contains(x.DrainagePointName)).Select(x => x.OilRate);
            var gasList = rates.Where(x => dpNames.Contains(x.DrainagePointName)).Select(x => x.GasRate);
            var waterList = rates.Where(x => dpNames.Contains(x.DrainagePointName)).Select(x => x.WaterRate);
            var condensateList = rates.Where(x => dpNames.Contains(x.DrainagePointName)).Select(x => x.CondensateRate);

            totalProd.AssetName = assetName;
            totalProd.CurrentDate = currentDate;

            avgProd.AssetName = assetName;
            avgProd.CurrentDate = currentDate;

            totalProd.CurrentOilRate = oilList.Aggregate();
            totalProd.CurrentGasRate = gasList.AggregateByMillion();
            totalProd.CurrentGasRateBOE = gasList.ConvertToBOE().Aggregate();
            totalProd.CurrentWaterRate = waterList.Aggregate();
            totalProd.CurrentCondensateRate = condensateList.Aggregate();

            avgProd.CurrentOilRate = oilList.ToAverage();
            avgProd.CurrentGasRate = gasList.AverageByMillion();
            avgProd.CurrentGasRateBOE = gasList.ToAverageBOE();
            avgProd.CurrentWaterRate = waterList.ToAverage();
            avgProd.CurrentCondensateRate = condensateList.ToAverage();
        }

        protected void ComputePreviousConsdensateRate(IEnumerable<string> dpNames, TotalRateData totalProd, AverageRateData avgProd, IEnumerable<ProductionRateData> prevConRates)
        {
            var condensateList = prevConRates.Where(x => dpNames.Contains(x.DrainagePointName)).Select(x => x.CondensateRate);

            totalProd.PreviousCondensateRate = condensateList.Aggregate();
            avgProd.PreviousCondensateRate = condensateList.ToAverage();
        }

        protected void ComputePreviousWaterRate(IEnumerable<string> dpNames, TotalRateData totalProd, AverageRateData avgProd, IEnumerable<ProductionRateData> prevWaterRates)
        {
            var waterList = prevWaterRates.Where(x => dpNames.Contains(x.DrainagePointName)).Select(x => x.WaterRate);

            totalProd.PreviousWaterRate = waterList.Aggregate();
            avgProd.PreviousWaterRate = waterList.ToAverage();
        }

        protected void ComputePreviousGasRate(IEnumerable<string> dpNames, TotalRateData totalProd, AverageRateData avgProd, IEnumerable<ProductionRateData> prevGasRates)
        {
            var gasList = prevGasRates.Where(x => dpNames.Contains(x.DrainagePointName)).Select(x => x.GasRate);
            totalProd.PreviousGasRate = gasList.AggregateByMillion();
            avgProd.PreviousGasRate = gasList.AverageByThousand();
        }

        protected void ComputePreviousOilRate(IEnumerable<string> dpNames, TotalRateData totalProd, AverageRateData avgProd, IEnumerable<ProductionRateData> prevOilRates)
        {
            var oilList = prevOilRates.Where(x => dpNames.Contains(x.DrainagePointName)).Select(x => x.OilRate);
            totalProd.PreviousOilRate = oilList.Aggregate();
            avgProd.PreviousOilRate = oilList.ToAverage();
        }

        public string ComputePercentageIncrease(double? currentValue, double? previousValue)
        {
            if (previousValue == null || currentValue == null) return null;

            var previous = Convert.ToDouble(previousValue);
            var current = Convert.ToDouble(currentValue);

            return previous > 0 ? $"{Math.Round((current-previous)/previous * 100, 2)} %" : null;
        }

        private (TotalRateData, AverageRateData) GetData(string assetName, IEnumerable<string> dpNames, IEnumerable<ProductionRateData> rates, DateTime? date,
                 PowerCollection<DateTime?, ProductionRateData> data)
        {
            var totalProd = new TotalRateData();
            var avgProd = new AverageRateData();

            ComputeCurrentProductionRate(assetName, dpNames, totalProd, avgProd, rates, date);

            totalProd.PreviousOilDate = GetPreviousDate(data, date, dpNames, ProductionDataVariablesEnum.Oil);
            totalProd.PreviousGasDate = GetPreviousDate(data, date, dpNames, ProductionDataVariablesEnum.Gas);
            totalProd.PreviousWaterDate = GetPreviousDate(data, date, dpNames, ProductionDataVariablesEnum.Water);
            totalProd.PreviousCondDate = GetPreviousDate(data, date, dpNames, ProductionDataVariablesEnum.Condensate);

            avgProd.PreviousOilDate = totalProd.PreviousOilDate;
            avgProd.PreviousGasDate = totalProd.PreviousGasDate;
            avgProd.PreviousWaterDate = totalProd.PreviousWaterDate;
            avgProd.PreviousCondDate = totalProd.PreviousCondDate;


            var prevOilRates = data[totalProd.PreviousOilDate];
            var prevGasRates = data[totalProd.PreviousGasDate];
            var prevWaterRates = data[totalProd.PreviousWaterDate];
            var prevConRates = data[totalProd.PreviousCondDate];

            ComputePreviousOilRate(dpNames, totalProd, avgProd, prevOilRates);
            ComputePreviousGasRate(dpNames, totalProd, avgProd, prevGasRates);
            ComputePreviousWaterRate(dpNames, totalProd, avgProd, prevWaterRates);
            ComputePreviousConsdensateRate(dpNames, totalProd, avgProd, prevConRates);


            return (totalProd, avgProd);
        }
        protected DateTime? GetPreviousDate(PowerCollection<DateTime?, ProductionRateData> data, DateTime? date, IEnumerable<string> dpNames, ProductionDataVariablesEnum @enum)
        {

            var dates = data.Where(x => dpNames.Contains(x.Value.DrainagePointName) && x.Value.Date != null)
                             .Select(x => x.Value.Date)
                             .OrderBy(x => x)
                             .Distinct()
                             .ToList();

            bool isInvalidDate = true;
            DateTime? invalidDate = date;
            DateTime? previousDate = null;

            while (isInvalidDate)
            {
                var index = dates.IndexOf(invalidDate);
                if (index == -1 || index <= 0) return null;

                previousDate = dates[index - 1];
                isInvalidDate = IsInvalidDate(data[previousDate], @enum);
                invalidDate = previousDate;

            }
            return previousDate;
        }

        private bool IsInvalidDate(IEnumerable<ProductionRateData> data, ProductionDataVariablesEnum @enum)
        {
            switch (@enum)
            {
                case ProductionDataVariablesEnum.Oil:
                    return !data.Any(x => x.OilRate > 0);
                case ProductionDataVariablesEnum.Gas:
                    return !data.Any(x => x.GasRate > 0);
                case ProductionDataVariablesEnum.Water:
                    return !data.Any(x => x.WaterRate > 0);
                case ProductionDataVariablesEnum.Condensate:
                    return !data.Any(x => x.CondensateRate > 0);
                default:
                    return true;
            }
        }

      
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ProductionDataService _dataService;
        protected readonly ProductionDataPath _dataPath;
        protected static TotalAvgRate defaultDotalAvgRate;
    }
}
