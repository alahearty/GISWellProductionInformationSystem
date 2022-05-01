using System.Collections.Generic;

namespace Orbit.Models.Adhoc
{
    public class WorkSheet: NamedEntity
    {
        public virtual IList<Column> Columns { get; }
        public virtual WorkSheetType Type { get;}

    }
}
