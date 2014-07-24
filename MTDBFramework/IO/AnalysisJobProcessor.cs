#region Namespaces

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MTDBFramework.Data;
using MTDBFramework.UI;

#endregion

namespace MTDBFramework.IO
{
	/// <summary>
	/// Processing for analysis jobs
	/// </summary>
    public class AnalysisJobProcessor : IProcessor
    {
        private int mCurrentItem;
        private int mTotalItems;
        private AnalysisJobItem mCurrentJob;
        private bool mAbortRequested;

        private PHRPReaderBase mAnalysisReader;

		/// <summary>
		/// Options
		/// </summary>
        public Options ProcessorOptions { get; set; }

		/// <summary>
		/// Support thread cancellation
		/// </summary>
        public void AbortProcessing()
        {
            mAbortRequested = true;
            mAnalysisReader.AbortProcessing();
        }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options"></param>
        public AnalysisJobProcessor(Options options)
        {
            ProcessorOptions = options;
        }

		/// <summary>
		/// Process Analysis Job Items according to their individual file types
		/// </summary>
		/// <param name="analysisJobItems">Analysis Job Items</param>
		/// <param name="bWorker"></param>
		/// <returns>Processed Analysis Job Items</returns>
        public IEnumerable<AnalysisJobItem> Process(IEnumerable<AnalysisJobItem> analysisJobItems, BackgroundWorker bWorker)
        {
            // analysisJobItems should have LcmsDataSet field be null
            mAbortRequested = false;

            mCurrentItem = 0;
            mTotalItems = analysisJobItems.Count();

            foreach (var jobItem in analysisJobItems)
            {
                if (bWorker.CancellationPending || mAbortRequested)
                    break;
                
                OnProgressChanged(new MtdbProgressChangedEventArgs(mCurrentItem, mTotalItems, jobItem));
                mCurrentJob = jobItem;

                mAnalysisReader = PhrpReaderFactory.Create(jobItem.FilePath, ProcessorOptions);

                mAnalysisReader.ProgressChanged += analysisReader_ProgressChanged;

                // Reads the jobItem using the reader returned by the Reader Factory
                jobItem.DataSet = mAnalysisReader.Read(jobItem.FilePath);

                mCurrentItem++;
                
            }
            
            return analysisJobItems;
        }

        private void analysisReader_ProgressChanged(object sender, PercentCompleteEventArgs e)
        {
            float percentComplete = (mCurrentItem * 100 + e.PercentComplete) / (mTotalItems * 100);

            var effectiveItemCount = (int)(mCurrentItem * 100 + e.PercentComplete);

            OnProgressChanged(new MtdbProgressChangedEventArgs(effectiveItemCount, mTotalItems * 100, e.CurrentTask, mCurrentJob));
        }

        #region Events

		/// <summary>
		/// Progress changed Event handler
		/// </summary>
        public event MtdbProgressChangedEventHandler ProgressChanged;

		/// <summary>
		/// Event handler
		/// </summary>
		/// <param name="e"></param>
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
