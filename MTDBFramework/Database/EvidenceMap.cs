#region Namespaces

using FluentNHibernate.Mapping;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Database
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
            Id(x => x.Id).Column("EvidenceId").GeneratedBy.Native();
            Map(x => x.Charge);
            Map(x => x.ObservedNet);
            Map(x => x.NetShift);
            Map(x => x.Mz);
            Map(x => x.Scan);
            Map(x => x.DelM);
            Map(x => x.DelMPpm);
            Map(x => x.SpecProb);

            References(x => x.Parent).Column("TargetId").Cascade.All();
            References(x => x.DataSet).Cascade.All();
        }
    }
}
