#region Namespaces

#endregion

using FluentNHibernate.Mapping;
using MTDBFrameworkBase.Data;

namespace MTDBFrameworkBase.Database
{
    /// <summary>
    /// This is our map for the ConsensusTarget class. Note the "Not.LazyLoad"
    /// </summary>
    public class ConsensusTargetMap : ClassMap<ConsensusTarget>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ConsensusTargetMap()
        {
            Table("Target");
            Not.LazyLoad();
            Id(x => x.Id).Column("TargetId").GeneratedBy.Assigned();
            Map(x => x.AverageNet);
            Map(x => x.StdevNet);
            Map(x => x.PredictedNet);
            Map(x => x.TheoreticalMonoIsotopicMass);
            Map(x => x.CleanSequence).Column("CleanSequence");
            Map(x => x.PrefixResidue);
            Map(x => x.SuffixResidue);
            Map(x => x.ModificationCount).Column("ModificationCount");
            Map(x => x.ModificationDescription).Column("ModificationDescription");
            Map(x => x.MultiProteinCount).Column("MultiProteinCount");

            HasMany(x => x.Evidences).KeyColumn("TargetId").Cascade.All();
        }
    }
}
