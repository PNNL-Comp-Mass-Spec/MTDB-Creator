using System;
using System.Windows.Forms;
using System.Collections.Generic;
using MTDBCreator.IO;
using MTDBCreator.Algorithms;
using MTDBCreator.Data;
using Regressor;
using MTDBCreator.Forms;
using System.Threading;
using System.IO;

namespace MTDBCreator
{
   

    /// <summary>
    /// Processes all of the analysis data objects to construct a MTBD
    /// </summary>
    public class MTDBProcessor: IDisposable
    {

        public event EventHandler<StatusEventArgs>              Status;
        public event EventHandler<StatusEventArgs>              Error;
        public event EventHandler<DatabaseCreatedEventArgs>     DatabaseCreated;
        public event EventHandler<DatabaseCreatedEventArgs>     AnalysisFailed;
        public event EventHandler<AnalysisCompletedEventArgs>   AnalysisCompleted;
        public event EventHandler<AnalysisCompletedEventArgs>   ProcessingAnalysis;


        /// <summary>
        /// Maps an analysis to a thread, that way if the analysis is running
        /// </summary>
        private Dictionary<Analysis, Thread> m_threadpool;

        private Thread          m_processingThread;
        private Options      p_options;
        private List<Analysis>  p_analyses;
        private MassTagDatabase p_database; 

        public MTDBProcessor()
        {
            m_processingThread = null;
            m_threadpool = new Dictionary<Analysis, Thread>();
        }

        private void OnStatus(string message)
        {
            if (Status != null)
            {
                Status(this, new StatusEventArgs(message));
            }
        }
        private void OnError(string message)
        {
            if (Error != null)
            {
                Error(this, new StatusEventArgs(message));
            }
        }

        private void AbortThread(Thread thread)
        {
            if (thread == null)
                return;

            try
            {
                thread.Abort();
                thread.Join(5000);
            }
            catch (ThreadAbortException)
            {
                // who cares!
            }
        }

        #region Processing      
  
        /// <summary>
        /// Creates a mass tag database given the associated analysis.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="analysisDescriptions"></param>
        public void CreateDatabase(Options options,
                                   List<Analysis> analysisDescriptions)
        {
            // kill anything running before
            AbortThread(m_processingThread);

            p_analyses          = analysisDescriptions;
            p_options           = options;
            ThreadStart start   = new ThreadStart(CreateDatabaseInternal);            
            m_processingThread  = new Thread(start);
            m_processingThread.Start();            
        }

        /// <summary>
        /// Threaded entry point for creating a mass tag database.
        /// </summary>
        private void CreateDatabaseInternal()
        {
            p_database = CreateDatabaseInternal(p_options, p_analyses);

            if (p_database == null)
            {
                return;
            }

            if (DatabaseCreated != null)
            {
                DatabaseCreated(this, new DatabaseCreatedEventArgs(p_database));
            }
        }

        /// <summary>
        /// Processes a list of dataset objects and returns a MTDB.
        /// </summary>        
        private MassTagDatabase CreateDatabaseInternal(  
                                                        Options         options,
                                                        List<Analysis>  analysisDescriptions)
        {                        
            MassTagDatabase massTagDatabase   = new MassTagDatabase();                       
            IRetentionTimePredictor predictor = RetentionTimePredictorFactory.CreatePredictor(options.PredictorType);
            bool success                      = true;
            
            // Analyze each of the datasets, putting the results into the mass tag database 
            // To expedite this process, only analyze if the analysis has not already been processed.
            int analysisNumber = 1;
            foreach(Analysis analysis in analysisDescriptions)
            {
                try
                {                    
                    if (analysis.ProcessedState != ProcessingState.Processed)
                    {
                        ProcessAnalysisJobInternal(analysis, options, predictor);
                    }

                    massTagDatabase.AddResults( analysis.Targets, 
                                                options.Regression, 
                                                predictor);

                    if (AnalysisCompleted != null)                    
                        AnalysisCompleted(this, new AnalysisCompletedEventArgs(analysisNumber++, analysisDescriptions.Count, analysis));                    
                }
                catch (IOException)
                {
                    analysis.ProcessedState = ProcessingState.NotProcessed;
                    success = false;
                    OnError(string.Format("Failed to open the analysis file {0}", analysis.FilePath));
                    break;
                }
                catch (AnalysisToolException)
                {
                    analysis.ProcessedState = ProcessingState.NotProcessed;
                    success = false;
                    OnError(string.Format("The analysis failed for {0}", analysis.Name));
                    break;
                }
                catch (Exception)
                {
                    analysis.ProcessedState = ProcessingState.NotProcessed;
                    success = false;
                    OnError(string.Format("The analysis failed for {0}", analysis.Name));
                    break;
                }
            }

            if (success)
            {
                massTagDatabase.FinalizeDatabase();                         
            }
            else
            {
                massTagDatabase = null;

                if (this.AnalysisFailed != null)
                {
                    AnalysisFailed(this, new DatabaseCreatedEventArgs(null));
                }
            }
            return massTagDatabase;
        }
        /// <summary>
        /// Filters a list of targets 
        /// </summary>
        /// <param name="targets"></param>
        /// <returns></returns>
        private List<Target> FilterTargets(ITargetFilter filter, List<Target> targets)
        {
            List<Target> newTargets = new List<Target>();
            foreach (Target t in targets)
            {
                bool shouldFilter = filter.ShouldFilter(t);
                if (!shouldFilter)
                {
                    newTargets.Add(t);
                }
            }
            return newTargets;
        }
        /// <summary>
        /// Processes an analysis job (SEQUEST or X!Tandem)
        /// </summary>
        /// <param name="analysis">Job to process</param>
        /// <param name="options">Options to use for processing.</param>
        /// <param name="massTagDatabase">Mass tag database to update.</param>
        /// <returns>True if successful, false if processing failed.</returns>
        public void ProcessAnalysisJob(Analysis analysis, Options options, IRetentionTimePredictor predictor)        
        {            
            lock (m_threadpool)
            {
                bool hasAnalysis = m_threadpool.ContainsKey(analysis);
                if (hasAnalysis) return;
                
                ParameterizedThreadStart start = new ParameterizedThreadStart(ProcessAnalysisJobInternal);                
                Thread analysisthread          = new Thread(start);

                AnalysisJob data = new AnalysisJob();
                data.Analysis    = analysis;
                data.Options     = options;
                data.Predictor   = predictor;
                analysisthread.Start(data);

                m_threadpool.Add(analysis, analysisthread);                 
            }
        }

        /// <summary>
        /// Thread entry point
        /// </summary>
        /// <param name="data"></param>
        private void ProcessAnalysisJobInternal(object data)
        {
            AnalysisJob analysisData = data as AnalysisJob;
            ProcessAnalysisJobInternal(analysisData.Analysis, analysisData.Options, analysisData.Predictor);

            // Once the thread is complete...kill the job
            lock (m_threadpool)
            {
                bool hasAnalysis = m_threadpool.ContainsKey(analysisData.Analysis);
                if (hasAnalysis)
                {
                    m_threadpool.Remove(analysisData.Analysis);
                }
            }

            if (AnalysisCompleted != null)
            {
                AnalysisCompleted(this, new AnalysisCompletedEventArgs(1, 1, analysisData.Analysis));
            }
        }
        private void ProcessAnalysisJobInternal(Analysis analysis, Options options, IRetentionTimePredictor predictor)        
        {
            analysis.ProcessedState = ProcessingState.Processing;
            if (this.ProcessingAnalysis != null)
            {
                ProcessingAnalysis(this, new AnalysisCompletedEventArgs(1, 1, analysis));
            }

            IAnalysisReader reader              = SequenceAnalysisToolsFactory.CreateReader(analysis.Tool);
            ITargetFilter filter                = TargetFilterFactory.CreateFilters(analysis.Tool, options);

            IRegressionAlgorithm    regressor   = RegressorFactory.CreateRegressor(RegressorType.LcmsRegressor, options.Regression);            
            Analysis results                    = reader.Read(analysis.FilePath, analysis.Name, filter);
            
            analysis.Proteins                   = results.Proteins; 
            analysis.Targets                    = results.Targets;            
            
            // Calculate the regression based on the target data
            List<float> predicted               = new List<float>();
            List<float> scans                   = new List<float>();
            foreach (Target target in analysis.Targets)
            {
                target.NetPredicted = predictor.GetElutionTime(target.CleanSequence);
                scans.Add(target.Scan);
                predicted.Add(Convert.ToSingle(target.NetPredicted));
            }
            analysis.RegressionResults           = regressor.CalculateRegression(scans, predicted);
            analysis.RegressionResults.Regressor = regressor;

            // Then make sure we align all against the predicted self
            regressor.ApplyTransformation(analysis.Targets);

            analysis.ProcessedState = ProcessingState.Processed;

        }       
        #endregion



        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                lock(m_threadpool)
                {
                    foreach(Analysis analysis in m_threadpool.Keys)
                    {
                        Thread thread = m_threadpool[analysis];
                        AbortThread(thread);
                    }
                }
            }
            catch
            {

            }
        }

        #endregion
    }

    /// <summary>
    /// Event arguments when a mass tag database is created.
    /// </summary>
    public class DatabaseCreatedEventArgs : EventArgs
    {
        public DatabaseCreatedEventArgs(MassTagDatabase database)
        {
            Database = database;
        }
        public MassTagDatabase  Database { get; private set; }
    }


    /// <summary>
    /// Event arguments when a mass tag database is created.
    /// </summary>
    public class AnalysisCompletedEventArgs : EventArgs
    {
        public AnalysisCompletedEventArgs(int analysisNumber, int total, Analysis analysis)
        {
            Analysis    = analysis;
            Number      = analysisNumber;
            Total       = total;
        }
        public Analysis Analysis { get; private set; }
        public int Number { get; private set; }
        public int Total { get; private set; }
    }

    public class AnalysisJob
    {
        public Analysis Analysis { get; set; }
        public Options Options { get; set; }
        public IRetentionTimePredictor Predictor { get; set; }
    }
}
