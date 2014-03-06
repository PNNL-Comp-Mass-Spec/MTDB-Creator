#region Namespaces

using FluentNHibernate.Mapping;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Database
{
    public class TargetMap : ClassMap<Target>
    {
        //This is our map for the Target class. Note the "Not.LazyLoad"
        public TargetMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Charge);
            Map(x => x.ObservedNet);
            Map(x => x.PredictedNet);
            Map(x => x.Mz);
            Map(x => x.MultiProteinCount);
            Map(x => x.Scan);
            Map(x => x.ModificationCount);
            Map(x => x.ModificationDescription);
            Map(x => x.DelM);
            Map(x => x.DelM_PPM);
            Map(x => x.SpecProb);

            References(x => x.DataSet).Cascade.SaveUpdate();
            References(x => x.PeptideInfo).Cascade.SaveUpdate();
        }
    }
}
