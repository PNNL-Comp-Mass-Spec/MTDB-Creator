#region Namespaces

using FluentNHibernate.Mapping;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Database
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
            Not.LazyLoad();
            Id(x => x.Id)
                .Column("ConsensusId").GeneratedBy.Native();
            Map(x => x.AverageNet);
            Map(x => x.StdevNet);
            Map(x => x.PredictedNet);
            Map(x => x.TheoreticalMonoIsotopicMass);
            Map(x => x.CleanSequence).Column("Sequence");
            Map(x => x.PrefixResidue);
            Map(x => x.SuffixResidue);
            Map(x => x.ModificationCount).Column("ModificationCount");
            Map(x => x.ModificationDescription).Column("ModificationDescription");
            Map(x => x.MultiProteinCount).Column("MultiProteinCount");

            //HasMany(x => x.PTMs).KeyColumn("ConsensusId").Cascade.All();
            HasMany(x => x.Evidences).Cascade.All();

        }
    }
}
