using Orbit.Application.Extensions;
using Orbit.Application.Services;
using Orbit.Application.Shared;
using Orbit.Models;
using Orbit.Models.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Application.ProductionRate.AverageRate
{
    public class AverageRateCommandHandler: BaseRate
    {
        public AverageRateCommandHandler(IUnitOfWork unitOfWork, ProductionDataService dataService,ProductionDataPath dataPath):base(unitOfWork,dataService,dataPath) { }

        public async Task<AverageRateDTO> GetDefaultAverageProductionRate()
        {
            var avgProd = new AverageRateDTO();
            var result= defaultDotalAvgRate;
            if(result == null)
                result = await GetDefaultProductionRate();
            if (result == null) return avgProd;



            avgProd.AverageRates = result.AverageRates;
            avgProd.PercentageIncreaseInOilRate = result.AvgPercentageIncreaseInOilRate;
            avgProd.PercentageIncreaseInGasRate = result.AvgPercentageIncreaseInGasRate;
            avgProd.PercentageIncreaseInWaterRate = result.AvgPercentageIncreaseInWaterRate;
            avgProd.PercentageIncreaseInCondensateRate = result.AvgPercentageIncreaseInCondensateRate;
            return avgProd;
        }

        public async Task<AverageRateDTO> GetAverageProductionRateByAsset(AverageRateCommand command)
        {
            var avgProd = new AverageRateDTO();

            if (IsInValidCommand(command)) return avgProd;

            switch (command.AssetType)
            {
                case AssetType.OML:
                    var fields = _unitOfWork.FieldRepository.FetchAllFields();
                    var allOMLs = fields.Where(i => command.Ids.Contains(i.OML)).Select(i => i.OML).Distinct();
                    var omlFields = fields.Where(x => command.Ids.Contains(x.OML.ToString()));

                    var omlResult = await GetProductionRateAtOML(_dataService.GetLastDate(_unitOfWork, _dataPath), allOMLs, omlFields);
                    if (omlResult == null) return avgProd;

                    avgProd.AverageRates = omlResult.AverageRates;
                    avgProd.PercentageIncreaseInOilRate = omlResult.AvgPercentageIncreaseInOilRate;
                    avgProd.PercentageIncreaseInGasRate = omlResult.AvgPercentageIncreaseInGasRate;
                    avgProd.PercentageIncreaseInWaterRate = omlResult.AvgPercentageIncreaseInWaterRate;
                    avgProd.PercentageIncreaseInCondensateRate = omlResult.AvgPercentageIncreaseInCondensateRate;
                    return avgProd;
                case AssetType.Field:
                    var afields = _unitOfWork.FieldRepository
                                    .FetchAllFields()
                                    .Where(x => command.Ids.Contains(x.Id.ToString()));

                    var fieldResult = await GetProductionRateAtAsset(_dataService.GetLastDate(_unitOfWork, _dataPath), afields);
                    if (fieldResult == null) return avgProd;

                    avgProd.AverageRates = fieldResult. AverageRates;
                    avgProd.PercentageIncreaseInOilRate = fieldResult.AvgPercentageIncreaseInOilRate;
                    avgProd.PercentageIncreaseInGasRate = fieldResult.AvgPercentageIncreaseInGasRate;
                    avgProd.PercentageIncreaseInWaterRate = fieldResult.AvgPercentageIncreaseInWaterRate;
                    avgProd.PercentageIncreaseInCondensateRate = fieldResult.AvgPercentageIncreaseInCondensateRate;
                    return avgProd;
                case AssetType.Reservoir:
                    var reservoirs = _unitOfWork.ReservoirRepository
                                   .FetchAllReservoirs()
                                   .Where(x => command.Ids.Contains(x.Id.ToString()));

                    var reservoirResult = await GetProductionRateAtAsset(_dataService.GetLastDate(_unitOfWork, _dataPath), reservoirs);
                    if (reservoirResult == null) return avgProd;

                    avgProd.AverageRates = reservoirResult.AverageRates;
                    avgProd.PercentageIncreaseInOilRate = reservoirResult.AvgPercentageIncreaseInOilRate;
                    avgProd.PercentageIncreaseInGasRate = reservoirResult.AvgPercentageIncreaseInGasRate;
                    avgProd.PercentageIncreaseInWaterRate = reservoirResult.AvgPercentageIncreaseInWaterRate;
                    avgProd.PercentageIncreaseInCondensateRate = reservoirResult.AvgPercentageIncreaseInCondensateRate;
                    return avgProd;

                case AssetType.DrainagePoint:
                    var dps = _unitOfWork.DrainagePointRepository
                                   .FetchAllDrainagepoints()
                                   .Where(x => command.Ids.Contains(x.Id.ToString()));

                    var dpResult = await GetProductionRateAtDrainagePoint(_dataService.GetLastDate(_unitOfWork, _dataPath), dps.Select(x => x.Name));
                    if (dpResult == null) return avgProd;

                    avgProd.AverageRates = dpResult.AverageRates;
                    avgProd.PercentageIncreaseInOilRate = dpResult.AvgPercentageIncreaseInOilRate;
                    avgProd.PercentageIncreaseInGasRate = dpResult.AvgPercentageIncreaseInGasRate;
                    avgProd.PercentageIncreaseInWaterRate = dpResult.AvgPercentageIncreaseInWaterRate;
                    avgProd.PercentageIncreaseInCondensateRate = dpResult.AvgPercentageIncreaseInCondensateRate;
                    return avgProd;

                default:
                    return avgProd;
            }
        }

        protected bool IsInValidCommand(AverageRateCommand command)
        {
            return command == null || command.Ids == null || command.Ids.NotAny();
        }
    }
}
