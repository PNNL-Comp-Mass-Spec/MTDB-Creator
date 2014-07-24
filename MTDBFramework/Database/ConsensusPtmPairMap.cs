using FluentNHibernate.Mapping;
using MTDBFramework.Data;

namespace MTDBFramework.Database
{
    /// <summary>
    /// Fluent NHibernate mapping for ConsensusPtmPair class
    /// </summary>
    public class ConsensusPtmPairMap : ClassMap<ConsensusPtmPair>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ConsensusPtmPairMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).Column("PairId").GeneratedBy.Native();
            Map(x => x.ConsensusId);
            Map(x => x.PtmId);
            Map(x => x.Location);
        }
    }
}
