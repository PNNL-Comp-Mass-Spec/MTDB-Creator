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
        private int m_currentItem;
        private int m_totalItems;
        private AnalysisJobItem m_currentJob;
        private bool m_abortRequested;

        private PHRPReaderBase m_analysisReader;

        /// <summary>
        /// Options
        /// </summary>
        public Options ProcessorOptions { get; set; }

        /// <summary>
        /// Support thread cancellation
        /// </summary>
        public void AbortProcessing()
        {
            m_abortRequested = true;
            m_analysisReader.AbortProcessing();
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
            m_abortRequested = false;

            m_currentItem = 0;
            m_totalItems = analysisJobItems.Count();

            foreach (var jobItem in analysisJobItems)
            {
                if (bWorker.CancellationPending || m_abortRequested)
                    break;

                OnProgressChanged(new MtdbProgressChangedEventArgs(m_currentItem, m_totalItems, jobItem));
                m_currentJob = jobItem;

                m_analysisReader = PhrpReaderFactory.Create(jobItem.FilePath, ProcessorOptions);

                m_analysisReader.ProgressChanged += analysisReader_ProgressChanged;

                // Reads the jobItem using the reader returned by the Reader Factory
                jobItem.DataSet = m_analysisReader.Read(jobItem.FilePath);

                m_currentItem++;

            }

            return analysisJobItems;
        }

        private void analysisReader_ProgressChanged(object sender, PercentCompleteEventArgs e)
        {
            float percentComplete = (m_currentItem * 100 + e.PercentComplete) / (m_totalItems * 100);

            var effectiveItemCount = (int)(m_currentItem * 100 + e.PercentComplete);

            OnProgressChanged(new MtdbProgressChangedEventArgs(effectiveItemCount, m_totalItems * 100, e.CurrentTask, m_currentJob));
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
            ProgressChanged?.Invoke(this, e);
        }

        #endregion

    }
}
