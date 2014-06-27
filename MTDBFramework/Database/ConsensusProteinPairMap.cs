using FluentNHibernate.Mapping;

namespace MTDBFramework.Database
{
    public class ConsensusProteinPairMap : ClassMap<ConsensusProteinPair>
    {
        public ConsensusProteinPairMap()
        {
            Not.LazyLoad();
            CompositeId().KeyReference(x => x.Consensus, "ConsensusId")
                        .KeyReference(x => x.Protein, "ProteinId");
            Map(x => x.CleavageState);
            Map(x => x.TerminusState);
            Map(x => x.ResidueStart);
            Map(x => x.ResidueEnd);
        }
    }
}
