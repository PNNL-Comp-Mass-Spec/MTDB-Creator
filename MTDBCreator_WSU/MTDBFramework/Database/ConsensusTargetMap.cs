#region Namespaces

using FluentNHibernate.Mapping;
using MTDBFramework.Algorithms.Clustering;

#endregion

namespace MTDBFramework.Database
{
    //This is our map for the ConsensusTarget class. Note the "Not.LazyLoad"
    public class ConsensusTargetMap : ClassMap<ConsensusTarget>
    {
        public ConsensusTargetMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Net);
            Map(x => x.StdevNet);
            Map(x => x.PredictedNet);
            Map(x => x.TheoreticalMonoIsotopicMass);
            Map(x => x.Sequence);

            HasMany(x => x.Targets).Cascade.SaveUpdate();
        }
    }
}
