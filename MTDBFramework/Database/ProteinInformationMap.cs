using FluentNHibernate.Mapping;
using MTDBFramework.Data;

namespace MTDBFramework.Database
{
    public class ProteinInformationMap : ClassMap<ProteinInformation>
    {
        public ProteinInformationMap()
        {
            Not.LazyLoad();
            Id(x => x.Id)
                .Column("ProteinId").GeneratedBy.Assigned();//Increment();
            Map(x => x.ProteinName);
            Map(x => x.CleavageState);
            Map(x => x.TerminusState);
            Map(x => x.ResidueStart);
            Map(x => x.ResidueEnd);

            HasManyToMany(x => x.Consensus).Table("ConsensusProteins")
                                           .ParentKeyColumn("ProteinId")
                                           .ChildKeyColumn("ConsensusId")
                                           .Cascade.All().Inverse();
             

            HasMany(x => x.ConsensusProtein).Cascade.All().Inverse().Table("ConsensusProtein");
        }
    }
}
