using FluentNHibernate.Mapping;
using Orbit.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orbit.NHibernate.Mappings
{
    public class BaseMap<T> : ClassMap<T> where T : BaseEntity
    {
        public BaseMap()
        {
            Id(x => x.Id).GeneratedBy.GuidComb();
        }

    }
}
