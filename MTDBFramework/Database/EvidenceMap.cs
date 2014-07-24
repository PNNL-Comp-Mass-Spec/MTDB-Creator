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
            Id(x => x.Id).Column("EvidenceId").GeneratedBy.Assigned();
            Map(x => x.Charge);
            Map(x => x.ObservedNet);
            //Map(x => x.PredictedNet);
            Map(x => x.Mz);
            //Map(x => x.MultiProteinCount);
            Map(x => x.Scan);
            Map(x => x.DelM);
            Map(x => x.DelMPpm);
            Map(x => x.SpecProb);

            References(x => x.Parent).Cascade.SaveUpdate();
            References(x => x.DataSet).Cascade.SaveUpdate();
        }
    }
}
