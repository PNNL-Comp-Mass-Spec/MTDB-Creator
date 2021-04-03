using MTDBFrameworkBase.Data;

namespace MTDBFrameworkBase.Database
{
    /// <summary>
    /// Target dataset information
    /// </summary>
    public class TargetDataSet
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Path only used to get the name of the data set for the database
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the name of the target dataset.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The input data format
        /// </summary>
        public LcmsIdentificationTool Tool { get; set; }
    }
}
