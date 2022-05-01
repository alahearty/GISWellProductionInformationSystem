using System.Collections.Generic;

namespace Orbit.Models.Adhoc
{
    public class WorkSheetType:NamedEntity
    {
        public virtual SheetType TypeName { get; }

        public virtual IList<Variable> Variables { get;  }
        public virtual IList<WorkSheet> WorkSheets { get; }
    }
}
