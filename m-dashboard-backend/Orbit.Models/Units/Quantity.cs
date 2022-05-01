using System.Collections.Generic;

namespace Orbit.Models.Units
{
    public partial class Quantity : NamedEntity
    {
        public virtual Unit BaseUnit { get; set; }
        public virtual IList<Unit> Units { get; set; }
    }
}