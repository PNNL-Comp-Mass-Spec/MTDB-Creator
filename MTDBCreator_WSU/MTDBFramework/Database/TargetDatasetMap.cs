using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using MTDBFramework.Data;

namespace MTDBFramework.Database
{
    // This is our map for the TargetDataSet class. Note the "Not.LazyLoad"
    // File path excluded because the information isn't relevant for if database
    // moves from one system to another (or if relevant file moves after writing)
    public class TargetDatasetMap : ClassMap<TargetDataSet>
    {
        public TargetDatasetMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name);

        }
    }
}
