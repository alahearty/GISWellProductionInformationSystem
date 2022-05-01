using Orbit.Models.Units;
using System.Collections.Generic;

namespace Orbit.Models.Adhoc
{
    public class Column: NamedEntity
    {
        public virtual WorkSheet WorkSheet { get; }
        public virtual Multiplier Multiplier { get;}
        public virtual Unit Unit { get;}
        public virtual IList<VariableMapping> ColumnMapping { get;}
    }
}
