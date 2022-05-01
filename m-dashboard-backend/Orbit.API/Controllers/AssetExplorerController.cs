using Microsoft.AspNetCore.Mvc;
using Orbit.Application.AssetExplorer;
using Orbit.Models.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Orbit.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetExplorerController : BaseController
    {
        public AssetExplorerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("home")]
        public async Task<ActionResult<IEnumerable<OMLExplorerItem>>> GetExplorerItems()
        {
            var data = await new AssetExplorerItemHandler().GetHomeAssetExplorerItems(_unitOfWork);
            return new ActionResult<IEnumerable<OMLExplorerItem>>(data);
        }
    }
}
