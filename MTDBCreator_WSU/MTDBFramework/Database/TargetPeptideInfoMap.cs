using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace MTDBFramework.Database 
{
    public class TargetPeptideInfoMap : ClassMap<TargetPeptideInfo>
    {
        public TargetPeptideInfoMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x./*PeptideInfo*/CleanPeptide);
            Map(x => x.Peptide/*InfoSequence*/);
            Map(x => x.PeptideWithNumericMods);
        }
         
    }
}
