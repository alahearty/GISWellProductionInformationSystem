using Orbit.Application.Extensions;
using Orbit.Application.Services;
using Orbit.Application.Shared;
using Orbit.Models;
using Orbit.Models.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Application.ProductionRate.TotalRate
{
    public class TotalRateCommandHandler:BaseRate
    {
        public TotalRateCommandHandler(IUnitOfWork unitOfWork, ProductionDataService dataService,ProductionDataPath dataPath) : base(unitOfWork, dataService,dataPath) { }

        public async Task<TotalRateDTO> GetDefaultTotalProductionRate()
        {
            var totalProd = new TotalRateDTO();

            var result = defaultDotalAvgRate;
            if (result == null)
                result = await GetDefaultProductionRate();
            if (result == null) return totalProd;

            totalProd.Dates = result.Dates;
            totalProd.TotalRates = result.TotalRates;
            totalProd.PercentageIncreaseInOilRate = result.TotalPercentageIncreaseInOilRate;
            totalProd.PercentageIncreaseInGasRate = result.TotalPercentageIncreaseInGasRate;
            totalProd.PercentageIncreaseInWaterRate = result.TotalPercentageIncreaseInWaterRate;
            totalProd.PercentageIncreaseInCondensateRate = result.TotalPercentageIncreaseInCondensateRate;

            return totalProd;
        }

        public async Task<TotalRateDTO> GetTotalProductionRateByDate(TotalRateCommand command)
        {
            var totalProd = new TotalRateDTO();

            if (IsInValidCommand(command)) return totalProd;

            switch (command.AssetType)
            {
                case AssetType.OML:
                    var fields = _unitOfWork.FieldRepository.FetchAllFields();
                    var allOMLs = fields.Where(i => command.Ids.Contains(i.OML)).Select(i => i.OML).Distinct();
                    var omlFields = fields.Where(x => command.Ids.Contains(x.OML));

                    var omlResult = await GetProductionRateAtOML(command.Date.DateOnly(), allOMLs, omlFields);
                    if (omlResult == null) return totalProd;

                    totalProd.Dates = omlResult.Dates;
                    totalProd.TotalRates = omlResult.TotalRates;
                    totalProd.PercentageIncreaseInOilRate = omlResult.TotalPercentageIncreaseInOilRate;
                    totalProd.PercentageIncreaseInGasRate = omlResult.TotalPercentageIncreaseInGasRate;
                    totalProd.PercentageIncreaseInWaterRate = omlResult.TotalPercentageIncreaseInWaterRate;
                    totalProd.PercentageIncreaseInCondensateRate = omlResult.TotalPercentageIncreaseInCondensateRate;
                    return totalProd;
                case AssetType.Field:
                    var afields = _unitOfWork.FieldRepository
                                    .FetchAllFields()
                                    .Where(x => command.Ids.Contains(x.Id.ToString()));

                    var fieldResult = await GetProductionRateAtAsset(command.Date.DateOnly(), afields);
                    if (fieldResult == null) return totalProd;

                    totalProd.Dates = fieldResult.Dates;
                    totalProd.TotalRates = fieldResult.TotalRates;
                    totalProd.PercentageIncreaseInOilRate = fieldResult.TotalPercentageIncreaseInOilRate;
                    totalProd.PercentageIncreaseInGasRate = fieldResult.TotalPercentageIncreaseInGasRate;
                    totalProd.PercentageIncreaseInWaterRate = fieldResult.TotalPercentageIncreaseInWaterRate;
                    totalProd.PercentageIncreaseInCondensateRate = fieldResult.TotalPercentageIncreaseInCondensateRate;
                    return totalProd;
                case AssetType.Reservoir:
                    var reservoirs = _unitOfWork.ReservoirRepository
                                   .FetchAllReservoirs()
                                   .Where(x => command.Ids.Contains(x.Id.ToString()));

                    var reservoirResult = await GetProductionRateAtAsset(command.Date.DateOnly(), reservoirs);
                    if (reservoirResult == null) return totalProd;

                    totalProd.Dates = reservoirResult.Dates;
                    totalProd.TotalRates = reservoirResult.TotalRates;
                    totalProd.PercentageIncreaseInOilRate = reservoirResult.TotalPercentageIncreaseInOilRate;
                    totalProd.PercentageIncreaseInGasRate = reservoirResult.TotalPercentageIncreaseInGasRate;
                    totalProd.PercentageIncreaseInWaterRate = reservoirResult.TotalPercentageIncreaseInWaterRate;
                    totalProd.PercentageIncreaseInCondensateRate = reservoirResult.TotalPercentageIncreaseInCondensateRate;
                    return totalProd;

                case AssetType.DrainagePoint:
                    var dps = _unitOfWork.DrainagePointRepository
                                   .FetchAllDrainagepoints()
                                   .Where(x => command.Ids.Contains(x.Id.ToString()));

                    var dpResult = await GetProductionRateAtDrainagePoint(command.Date.DateOnly(), dps.Select(x => x.Name));
                    if (dpResult == null) return totalProd;

                    totalProd.Dates = dpResult.Dates;
                    totalProd.TotalRates = dpResult.TotalRates;
                    totalProd.PercentageIncreaseInOilRate = dpResult.TotalPercentageIncreaseInOilRate;
                    totalProd.PercentageIncreaseInGasRate = dpResult.TotalPercentageIncreaseInGasRate;
                    totalProd.PercentageIncreaseInWaterRate = dpResult.TotalPercentageIncreaseInWaterRate;
                    totalProd.PercentageIncreaseInCondensateRate = dpResult.TotalPercentageIncreaseInCondensateRate;
                    return totalProd;

                default:
                    return totalProd;
            }
        }
        private bool IsInValidCommand(TotalRateCommand command)
        {
            return command == null || command.Date == null || command.Ids == null || command.Ids.NotAny();
        }
    }
}
