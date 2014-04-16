using FluentNHibernate.Mapping;

namespace MTDBFramework.Database 
{
    //This is our map for the TargetPeptideInfo class. Note the "Not.LazyLoad"
    public class TargetPeptideInfoMap : ClassMap<TargetPeptideInfo>
    {
        public TargetPeptideInfoMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.CleanPeptide);
            Map(x => x.Peptide);
            Map(x => x.PeptideWithNumericMods);
        }
         
    }
}
