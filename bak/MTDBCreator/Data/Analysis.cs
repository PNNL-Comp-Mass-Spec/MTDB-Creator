using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Data
{
    public enum ProcessingState
    {
        Processing,
        Processed,
        NotProcessed
    }

    /// <summary>
    /// Maintains the analysis meta-data and results.
    /// </summary>
    public class Analysis
    {
        private int m_featureCount;

        /// <summary>
        /// Constructor
        /// </summary>
        public Analysis()
        {
            RegressionResults   = null;
            Targets             = new List<Target>();
            Proteins            = new List<Protein>();
            ProcessedState      = ProcessingState.NotProcessed;
            Id                  = 0;
            m_featureCount      = 0;
            CreationDate        = DateTime.Now;
        }

        public Analysis(Analysis analysis)
        {
            m_featureCount = analysis.NumberOfUniqueMassTags;
            Name           = analysis.Name;
            FilePath       = analysis.FilePath;
            CreationDate   = DateTime.Now;
            Id             = analysis.Id;
            m_featureCount = 0;
            ProcessedState = analysis.ProcessedState;
        }
        public DateTime CreationDate { get; private set; }
        /// <summary>
        /// Gets or sets the ID of the analysis.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the flag whether the dataset has been processed.
        /// </summary>
        public ProcessingState ProcessedState { get; set; }
        /// <summary>
        /// Results from the regression
        /// </summary>
        public RegressionResult RegressionResults { get; set; }
        /// <summary>
        /// Gets the number of unique mass tags
        /// </summary>
        public int NumberOfUniqueMassTags
        {
            get
            {
                return m_featureCount;
            }
        }
        /// <summary>
        /// Gets or sets the proteins
        /// </summary>
        public List<Protein> Proteins { get; set; }
        /// <summary>
        /// Gets or sets the targets discovered by this tool
        /// </summary>
        public List<Target> Targets { get; private set; }

        public void AddTargets(List<Target> targets)
        {
            Targets         = targets;
            m_featureCount  = targets.Count;
        }

        /// <summary>
        /// Gets or sets the file path of the analysis.
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// Gets or sets the name of the dataset.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the tool this analysis derived from.
        /// </summary>
        public SupportedTools Tool { get; set; }

        public  void Clear()
        {
            Targets.Clear();
            Proteins.Clear();
            RegressionResults = null;
            ProcessedState = ProcessingState.NotProcessed;
            m_featureCount = 0;
        }
    }
}
