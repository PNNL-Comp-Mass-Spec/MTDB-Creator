using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using MTDBFramework.Data;

namespace MTDBFramework.Database
{
    public class TargetDatasetMap : ClassMap<TargetDataSet>
    {
        public TargetDatasetMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Path);

        }
    }
}
