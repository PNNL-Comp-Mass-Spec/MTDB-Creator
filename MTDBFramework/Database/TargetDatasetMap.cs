using FluentNHibernate.Mapping;

namespace MTDBFramework.Database
{
    // This is our map for the TargetDataSet class. Note the "Not.LazyLoad"
    // File path excluded because the information isn't relevant for if database
    // moves from one system to another (or if relevant file moves after writing)
    public class TargetDatasetMap : ClassMap<TargetDataSet>
    {
        public TargetDatasetMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).Column("DatasetId").GeneratedBy.Identity();
            Map(x => x.Name);
            Map(x => x.Tool).Column("SearchTool");
        }
    }
}
