

namespace Orbit.Models
{
    public abstract class NamedEntity : BaseEntity
    {
        public virtual string Name { get; set; }
        public override string ToString() => Name;
    }
}
