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
        private int mCurrentItem;
        private int mTotalItems;
        private AnalysisJobItem mCurrentJob;

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

            mCurrentItem = 0;
            mTotalItems = analysisJobItems.Count();

            foreach (var jobItem in analysisJobItems)
            {
                OnProgressChanged(new MtdbProgressChangedEventArgs(mCurrentItem, mTotalItems, jobItem));
                mCurrentJob = jobItem;

                PHRPReaderBase analysisReader = PhrpReaderFactory.Create(jobItem.FilePath, ProcessorOptions);

                analysisReader.ProgressChanged += analysisReader_ProgressChanged;
                
				// Reads the jobItem using the reader returned by the Reader Factory
                jobItem.DataSet = analysisReader.Read(jobItem.FilePath);

                mCurrentItem++;
            }
            
            return analysisJobItems;
        }

        private void analysisReader_ProgressChanged(object sender, PercentCompleteEventArgs e)
        {
            float percentComplete = (mCurrentItem * 100 + e.PercentComplete) / (mTotalItems * 100);

            var effectiveItemCount = (int)(mCurrentItem * 100 + e.PercentComplete);

            OnProgressChanged(new MtdbProgressChangedEventArgs(effectiveItemCount, mTotalItems * 100, mCurrentJob));
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
