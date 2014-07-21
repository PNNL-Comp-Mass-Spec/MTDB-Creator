using FluentNHibernate.Mapping;
using MTDBFramework.Data;

namespace MTDBFramework.Database
{
    public class ProteinInformationMap : ClassMap<ProteinInformation>
    {
        public ProteinInformationMap()
        {
            Not.LazyLoad();
            Id(x => x.Id)
                .Column("ProteinId").GeneratedBy.Native();
            Map(x => x.ProteinName);
        }
    }
}
