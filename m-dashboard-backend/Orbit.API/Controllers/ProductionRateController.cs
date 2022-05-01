using Microsoft.AspNetCore.Mvc;
using Orbit.Application.ProductionRate.AverageRate;
using Orbit.Application.ProductionRate.TotalRate;
using Orbit.Application.ProductionRate.WellRate;
using Orbit.Application.Services;
using Orbit.Application.Shared;
using Orbit.Models.Repositories;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orbit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionRateController : BaseController
    {
        public ProductionRateController(ProductionDataService dataService, IUnitOfWork unitOfWork, ProductionDataPath dataPath)
        {
            _dataService = dataService;
            _unitOfWork = unitOfWork;
            _dataPath = dataPath;
        }
        [HttpGet("average-rate")]
        public async Task<ActionResult<AverageRateDTO>> GetDefaultAverageProductionRate()
        {
            var handler = new AverageRateCommandHandler(_unitOfWork, _dataService, _dataPath);
            var data = await handler.GetDefaultAverageProductionRate();
            return new ActionResult<AverageRateDTO>(data);
        }

        [HttpPost("average-rate-asset")]
        public async Task<ActionResult<AverageRateDTO>> GetAverageProductionRateByAsset([FromBody] AverageRateCommand command)
        {
            var handler = new AverageRateCommandHandler(_unitOfWork, _dataService, _dataPath);
            var data = await handler.GetAverageProductionRateByAsset(command);
            return new ActionResult<AverageRateDTO>(data);
        }

        [HttpGet("total-rate")]
        public async Task<ActionResult<TotalRateDTO>> GetDefaultTotalProductionRate()
        {
            var handler = new TotalRateCommandHandler(_unitOfWork, _dataService, _dataPath);
            var data = await handler.GetDefaultTotalProductionRate();
            return new ActionResult<TotalRateDTO>(data);
        }

        [HttpPost("total-rate-date")]
        public async Task<ActionResult<TotalRateDTO>> GetTotalProductionRateByDate([FromBody] TotalRateCommand command)
        {
            var handler = new TotalRateCommandHandler(_unitOfWork, _dataService, _dataPath);
            var data = await handler.GetTotalProductionRateByDate(command);
            return new ActionResult<TotalRateDTO>(data);
        }

        [HttpPost("well-rate")]
        public async Task<ActionResult<IEnumerable<WellRateDTO>>> GetWellProductionRateByDate([FromBody] WellRateCommand command)
        {
            var handler = new WellRateCommandHandler(_unitOfWork, _dataService, _dataPath);
            var data = await handler.GetWellTotalProductionRateByDrainagePoint(command);
            return new ActionResult<IEnumerable<WellRateDTO>>(data);
        }

        private readonly ProductionDataService _dataService;
        private readonly ProductionDataPath _dataPath;
    }
}
