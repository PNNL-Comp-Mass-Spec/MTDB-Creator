using FluentNHibernate.Mapping;
using MTDBFramework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBFramework.Database
{
    public class ProteinInformationMap : ClassMap<ProteinInformation>
    {
        public ProteinInformationMap()
        {
            Not.LazyLoad();
            Id(x => x.Id)
                .Column("ProteinId").GeneratedBy.Increment();
            Map(x => x.ProteinName);
            Map(x => x.CleavageState);
            Map(x => x.TerminusState);
            Map(x => x.ResidueStart);
            Map(x => x.ResidueEnd);

            HasManyToMany(x => x.Consensus).Table("ConsensusProteins")
                                           .ParentKeyColumn("ProteinId")
                                           .ChildKeyColumn("ConsensusId")
                                           .Cascade.All().Inverse();
        }
    }
}
