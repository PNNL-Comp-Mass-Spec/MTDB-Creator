#region Namespaces

#endregion

using FluentNHibernate.Mapping;
using MTDBFrameworkBase.Data;

namespace MTDBFrameworkBase.Database
{
    /// <summary>
    /// NHibernate mapping for the Evidence class. Note the "Not.LazyLoad"
    /// </summary>
    public class EvidenceMap : ClassMap<Evidence>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EvidenceMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).Column("EvidenceId").GeneratedBy.Assigned();
            Map(x => x.Charge);
            Map(x => x.ObservedNet);
            Map(x => x.NetShift);
            Map(x => x.Mz);
            Map(x => x.Scan);
            Map(x => x.DelM);
            Map(x => x.DelMPpm);
            Map(x => x.DiscriminantValue).Column("QValue");
            Map(x => x.SpecProb);

            References(x => x.Parent).Column("TargetId").Cascade.All();
            References(x => x.DataSet).Cascade.All();
        }
    }
}
