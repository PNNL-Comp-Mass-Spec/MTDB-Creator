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
                .Column("ProteinId").GeneratedBy.Assigned();
            Map(x => x.ProteinName);
        }
    }
}
