namespace Orbit.Models.Assets
{
    public class DrainagePoint : BaseEntity
    {
        public virtual string Name => $"{Well.Field.Name}-{Well.WellCode}{StringCode}:{Reservoir.Name}";
        public virtual Well Well {get;}
        public virtual Reservoir Reservoir {get;}
        public virtual string StringCode { get;}
       
        public override string ToString() => Name;
    }
}
