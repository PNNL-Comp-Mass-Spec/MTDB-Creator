#region Namespaces

using FluentNHibernate.Mapping;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Database
{
    //This is our map for the ConsensusTarget class. Note the "Not.LazyLoad"
    public class ConsensusTargetMap : ClassMap<ConsensusTarget>
    {
        public ConsensusTargetMap()
        {
            Not.LazyLoad();
            Id(x => x.Id)
                .Column("ConsensusId").GeneratedBy.Assigned();
            Map(x => x.Net);
            Map(x => x.StdevNet);
            Map(x => x.PredictedNet);
            Map(x => x.TheoreticalMonoIsotopicMass);
            Map(x => x.SeqWithNumericMods).Column("Sequence");
            Map(x => x.PrefixResidue);
            Map(x => x.SuffixResidue);
            Map(x => x.ModificationCount).Column("ModificationCount");
            Map(x => x.ModificationDescription).Column("ModificationDescription");
            Map(x => x.MultiProteinCount).Column("MultiProteinCount");


            HasMany(x => x.Evidences).KeyColumn("ConsensusId").Cascade.All();

        }
    }
}
