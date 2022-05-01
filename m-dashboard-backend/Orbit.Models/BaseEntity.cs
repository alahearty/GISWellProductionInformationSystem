using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.Models
{
    public abstract class BaseEntity
    {
        public virtual Guid Id { get; set; }
    }
}
