using Orbit.Models.Adhoc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Models.Repositories
{
    public interface IWorkSheetRepository
    {
        IList<WorkSheet> FetchAllWorkSheets();
    }
}
