using System;
using System.Collections.Generic;
using System.Text;

namespace MTDBCreator
{
    public class ProcessArguments
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="options">Analysis options.</param>
        /// <param name="analysisDescriptions">List of analysis job descriptions.</param>
        /// <param name="writeToAccessDatabase">Flag when true will write to an MS Access database. When false will create a set of temporary files.</param>
        /// <param name="databasePath">Path of database to create.</param>
        public ProcessArguments(clsOptions options,
                                List<clsAnalysisDescription> analysisDescriptions,
                                bool writeToAccessDatabase,
                                string databasePath)
        {
            Options                 = options;
            JobDescriptions         = analysisDescriptions;
            WriteToAccessDatabase   = writeToAccessDatabase;
            DatabasePath            = databasePath;
        }
        /// <summary>
        /// Gets or sets the regression and analysis options.
        /// </summary>
        public clsOptions Options { get; set; }
        /// <summary>
        /// Gets or sets the list of analysis job descriptions.
        /// </summary>
        public List<clsAnalysisDescription> JobDescriptions { get; set; }
        /// <summary>
        /// Gets or sets the flag indicating whether to write to the database or not.
        /// </summary>
        public bool WriteToAccessDatabase { get; set; }
        /// <summary>
        /// Gets or sets the path of the database to save to.
        /// </summary>
        public string DatabasePath { get; set; }
    }
}
