#region Namespaces

using System.Collections.Generic;
using System.Linq;
using MTDBFramework.Data;
using MTDBFramework.UI;

#endregion

namespace MTDBFramework.IO
{
    public class AnalysisJobProcessor : IProcessor
    {
        public Options ProcessorOptions { get; set; }

        public AnalysisJobProcessor(Options options)
        {
            ProcessorOptions = options;
        }

		// Entry point for processing analysis job items. Accepts a IEnumerable of Analysis Job Items
		// and returns the same.
		// It will Analyse each one individually depending on the file type using PHRP Reader
        public IEnumerable<AnalysisJobItem> Process(IEnumerable<AnalysisJobItem> analysisJobItems)
        {
            // analysisJobItems should have LcmsDataSet field be null

            var current = 0;
            var total = analysisJobItems.Count();

            foreach (var jobItem in analysisJobItems)
            {
                OnProgressChanged(new MtdbProgressChangedEventArgs(current, total, jobItem));

                // Legacy code from before implementation of clsPHRPReader
                // IAnalysisReader analysisReader = AnalysisReaderFactory.Create(jobItem.Format, this.ProcessorOptions);

                var analysisReader = PhrpReaderFactory.Create(jobItem.FilePath, ProcessorOptions);

				// Reads the jobItem using the reader returned by the Reader Factory
                jobItem.DataSet = analysisReader.Read(jobItem.FilePath);

                current++;
            }
            
            return analysisJobItems;
        }

        #region Events

        public event MtdbProgressChangedEventHandler ProgressChanged;

        protected void OnProgressChanged(MtdbProgressChangedEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
            }
        }

        #endregion
    }
}
