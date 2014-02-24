#region Namespaces

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MTDBFramework.Algorithms.Alignment;
using MTDBFramework.Data;
using MTDBFramework.UI;
using Regressor.Algorithms;

#endregion

namespace MTDBFramework.IO
{
    public class AnalysisJobProcessor : IProcessor
    {
        public Options ProcessorOptions { get; set; }

        public AnalysisJobProcessor(Options options)
        {
            this.ProcessorOptions = options;
        }

        public IEnumerable<AnalysisJobItem> Process(IEnumerable<AnalysisJobItem> analysisJobItems)
        {
            // analysisJobItems should have LcmsDataSet field be null

            int current = 0;
            int total = analysisJobItems.Count();

            List<AnalysisJobItem> phrps = new List<AnalysisJobItem>();
            List<AnalysisJobItem> jobs = new List<AnalysisJobItem>();


            foreach (AnalysisJobItem jobItem in analysisJobItems)
            {
                OnProgressChanged(new MTDBProgressChangedEventArgs(current, total, jobItem));

                // Legacy code from before implementation of clsPHRPReader
                // IAnalysisReader analysisReader = AnalysisReaderFactory.Create(jobItem.Format, this.ProcessorOptions);

                IPHRPReader analysisReader = PHRPReaderFactory.Create(jobItem.FilePath, this.ProcessorOptions);

                jobItem.DataSet = analysisReader.Read(jobItem.FilePath);

                current++;
            }
            
            return analysisJobItems;
        }

        #region Events

        public event MTDBProgressChangedEventHandler ProgressChanged;

        protected void OnProgressChanged(MTDBProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }

        #endregion
    }
}
