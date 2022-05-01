namespace Orbit.Models.Units
{
    public partial class UnitProperty : NamedEntity
    {
        public virtual Unit DatabaseUnit { get; }
        public virtual Unit DefaultUnit { get; set; }
        public virtual Multiplier DatabaseUnitMultiplier { get;}
        public virtual Multiplier DefaultUnitMultiplier { get;}
        public virtual string Category { get;}
    }
}
