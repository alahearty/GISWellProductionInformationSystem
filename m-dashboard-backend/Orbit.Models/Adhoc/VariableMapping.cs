namespace Orbit.Models.Adhoc
{
    public class VariableMapping: NamedEntity
    {
        private Variable variable;
        public virtual Variable Variable
        {
            get { return variable; }
            set
            {
                variable = value;
                if (value != null)
                    value.Mapping = this;
            }
        }
        public virtual Column TargetColumn { get; set; }
    }
}
