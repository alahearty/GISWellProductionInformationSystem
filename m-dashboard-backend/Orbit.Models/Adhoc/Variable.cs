using System;
using System.Collections.Generic;
using System.Text;
using Orbit.Models.Units;

namespace Orbit.Models.Adhoc
{
    public class Variable:NamedEntity
    {
        public virtual WorkSheetType TableType { get; set; }
        public virtual Unit DefaultUnit { get; set; }
        public virtual Multiplier DefaultMultiplier { get; set; }
        public virtual VariableDataType DataType { get; set; }
        public virtual VariableMapping Mapping { get; set; }
    }
}
