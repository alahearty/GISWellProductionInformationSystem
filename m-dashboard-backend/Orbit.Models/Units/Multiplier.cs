

namespace Orbit.Models.Units
{
    public class Multiplier : BaseEntity
    {
        public virtual string MultiplierSymbol {get;}
        public virtual double Multiplicand {get;}
        public virtual Unit Unit {get;}
        public override string ToString() => MultiplierSymbol;
    }
}