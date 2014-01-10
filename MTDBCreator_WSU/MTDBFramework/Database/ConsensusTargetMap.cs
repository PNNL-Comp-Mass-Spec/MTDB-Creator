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
            Map(x => x.Mass);
            Map(x => x.StdevMass);
            References(x => x.Dataset).Cascade.SaveUpdate();
            HasMany(x => x.Targets).Cascade.SaveUpdate();
        }
    }
}
