using FluentNHibernate.Mapping;

namespace MTDBFramework.Database
{
    /// <summary>
    /// NHibernate mapping for the TargetDataSet class. Note the "Not.LazyLoad"
    /// File path excluded because the information isn't relevant for if database
    /// moves from one system to another (or if relevant file moves after writing)
    /// </summary>
    public class TargetDatasetMap : ClassMap<TargetDataSet>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TargetDatasetMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).Column("DatasetId").GeneratedBy.Assigned();
            Map(x => x.Name);
            Map(x => x.Tool).Column("SearchTool");
        }
    }
}
