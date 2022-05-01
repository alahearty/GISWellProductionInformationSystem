using Orbit.Models.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orbit.Application.AssetExplorer
{
    public class AssetExplorerItemHandler
    {
        public async Task<IEnumerable<OMLExplorerItem>> GetHomeAssetExplorerItems(IUnitOfWork unitOfWork)
        {
            return await Task.Run(() =>
            {
                var fields = unitOfWork.FieldRepository.FetchAllFields();
                var allOMLs = fields.Select(i => i.OML).Distinct();
                return allOMLs.Select((oml) => new OMLExplorerItem(oml, fields));
            });
        }
    }
}
