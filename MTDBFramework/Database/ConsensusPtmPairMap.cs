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
            Map(x => x.Location);

            References(x => x.Target).Column("TargetId").Cascade.All();

            References(x => x.PostTranslationalModification).Column("PostTranslationModId").Cascade.All();
        }
    }
}
