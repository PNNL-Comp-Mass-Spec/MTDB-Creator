using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace MTDBCreator
{
   

    /// <summary>
    /// Processes all of the analysis data objects to construct a MTBD
    /// </summary>
    public class MTDBProcessor: ProcessorBase
    {
        public const string CONST_ANALYSIS_TOOL_SEQUEST = "SEQUEST";
        public const string CONST_ANALYSIS_TOOL_XTANDEM = "XTANDEM";

        public event EventHandler ProcessingFailed;
        public event DelegateSetPercentComplete                 TotalProgressComplete;
        public event EventHandler<ProcessingCompleteEventArgs>  ProcessingComplete;
        /// <summary>
        /// Enumeration stating whether to continue processing the data or not.
        /// </summary>
        private enum AnalysisProcessResult { Success, FailedButIgnore, FailedStopProcessing };


        #region Processing
        /// <summary>
        /// Processes analysis thread start location.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public void ProcessAnalysis(object o)
        {
            

            ProcessArguments arguments = o as ProcessArguments;
            if (arguments == null)
                return;

            ProcessAllDatasets( arguments.Options,
                                arguments.JobDescriptions,
                                arguments.WriteToAccessDatabase,
                                arguments.DatabasePath);
        }
        /// <summary>
        /// Processes a list of dataset objects and returns a MTDB.
        /// </summary>
        public clsMTDB ProcessAllDatasets(  clsOptions options,
                                            List<clsAnalysisDescription> analysisDescriptions,
                                            bool writeToAccessDatabase,
                                            string databasePath
                                          )
        {            
            clsMTDB massTagDatabase = new clsMTDB(options);
            RegisterProcessing(massTagDatabase);
            StatusMessage("Processing Datasets.");

            bool success = true;
            int i = 0;
            //  A list of datasets that had an IO error.
            List<string> failed = new List<string>();
            foreach(clsAnalysisDescription description in analysisDescriptions)
            {
                try
                {
                    ProcessAnalysisJob(description, options, ref  massTagDatabase);

                    int percent = Convert.ToInt32(Convert.ToDouble(++i) * 100.0 / Convert.ToDouble(analysisDescriptions.Count));
                    if (TotalProgressComplete != null)
                        TotalProgressComplete(percent);

                }
                catch (System.IO.IOException io)
                {
                    //TODO: Do we continue?
                    failed.Add(description.mstrDataset);
                    ErrorMessage("Could not read " + description.mstrDataset + ".  There was an error. " + io.Message);                    
                }
                catch (AnalysisToolException ex)
                {
                    success = false;
                    ErrorMessage("Analysis tool exception thrown.  " + ex.Message);
                    break;
                }
                catch (Exception ex)
                {
                    success = false;
                    ErrorMessage("There was an error processing " + description.mstrDataset + "." + ex.Message);
                    break;
                }
            }

            if (success)
            {
                massTagDatabase.CalculateMassTagNETs();
                massTagDatabase.CalculateProteinsPassingFilters();

                StatusMessage("Saving database.");
                massTagDatabase.LoadResultsIntoDB(writeToAccessDatabase, databasePath);

                if (ProcessingComplete != null)
                    ProcessingComplete(this, new ProcessingCompleteEventArgs("Complete.", massTagDatabase, failed));                            
            }
            else
            {
                massTagDatabase = null;
                if (ProcessingFailed != null)
                    ProcessingFailed(this, new EventArgs());
            }

            return massTagDatabase;
        }

        /// <summary>
        /// Processes an analysis job (SEQUEST or X!Tandem)
        /// </summary>
        /// <param name="analysis">Job to process</param>
        /// <param name="options">Options to use for processing.</param>
        /// <param name="massTagDatabase">Mass tag database to update.</param>
        /// <returns>True if successful, false if processing failed.</returns>
        private void ProcessAnalysisJob(clsAnalysisDescription analysis, clsOptions options, ref clsMTDB massTagDatabase)
        {            
            analysis.mdbl_scan_net_slope        = 0;
            analysis.mdbl_scan_net_intercept    = 0;

            if (analysis.mstrAnalysisTool.ToUpper() == CONST_ANALYSIS_TOOL_XTANDEM)
            {
                ProcessXTandemFile(analysis, options, ref massTagDatabase);                    
            }
            else if (analysis.mstrAnalysisTool.ToUpper() == CONST_ANALYSIS_TOOL_SEQUEST)
            {
                ProcessSequestFile(analysis, options, ref massTagDatabase);                    
            }                                       
        }

        private void ProcessSequestFile(clsAnalysisDescription analysis, 
                                                    clsOptions options,
                                                    ref clsMTDB massTagDatabase)
        {
            DateTime now        = DateTime.Now;
            double slope        = 0;
            double intercept    = 0;
            double rsquared     = 0;
            int numScans        = 0;


            float[] peptideScans  = new float[1];
            float[] predictedNETs = new float[1];

            StatusMessage("Reading PHRP files for " + analysis.mstrDataset);
            clsSequestAnalysisReader sequestPHRPResultsReader = new clsSequestAnalysisReader(analysis.mstrArchivePath, analysis.mstrDataset);
            RegisterProcessing(sequestPHRPResultsReader);

            DateTime read = DateTime.Now;
            StatusMessage("Performing Alignment");

            massTagDatabase.AlignSequestDatasetToTheoreticalNETs(sequestPHRPResultsReader,
                                                          ref peptideScans, 
                                                          ref predictedNETs, 
                                                          ref slope,
                                                          ref intercept, 
                                                          ref numScans, 
                                                          ref rsquared,
                                                          options.UseKrokhinNET);

            //TODO: What is this doing?
            //DisplayScansVsNet(analysis.mstrDataset);
            
            analysis.mdbl_scan_net_intercept = intercept;
            analysis.mdbl_scan_net_slope     = slope;
            analysis.mint_num_scans          = numScans;
            analysis.mdbl_scan_net_rsquared  = rsquared;

            analysis.mint_num_unique_mass_tags = sequestPHRPResultsReader.marrSeqInfo.Length;
            massTagDatabase.AddResults(sequestPHRPResultsReader, (Regressor.RegressionType)options.RegressionType, analysis);


            RegressionResult result = new RegressionResult( analysis.mstrDataset,
                                                            analysis.mdbl_scan_net_slope,
                                                            analysis.mdbl_scan_net_intercept,
                                                            analysis.mdbl_scan_net_rsquared,
                                                            analysis.mint_num_unique_mass_tags);

            DateTime add = DateTime.Now;
            Console.WriteLine("# of items = " + sequestPHRPResultsReader.marrSequestResults.Length + " Read Time = " + Convert.ToString(read.Subtract(now)) + " Insert time = " + Convert.ToString(add.Subtract(read)));
        }
       
        private void ProcessXTandemFile(clsAnalysisDescription analysis,
                                                    clsOptions options,
                                                    ref clsMTDB massTagDatabase)
        {
            DateTime now     = DateTime.Now;
            double slope     = 0;
            double intercept = 0;
            double rsquared  = 0;
            int numScans     = 0;

            StatusMessage("Reading PHRP files for " + analysis.mstrDataset);

            clsXTandemAnalysisReader xTandemPHRPResultsReader = new clsXTandemAnalysisReader(analysis.mstrArchivePath,
                                                                                             analysis.mstrDataset);
            

            DateTime read = DateTime.Now;
            StatusMessage("Performing Alignment");

            float[] peptideScans = new float[1];
            float[] predictedNETs = new float[1];
            
            massTagDatabase.AlignXTandemDatasetToTheoreticalNETs(  xTandemPHRPResultsReader,
                                                            ref peptideScans, 
                                                            ref predictedNETs, 
                                                            ref slope,
                                                            ref intercept, 
                                                            ref numScans, 
                                                            ref rsquared,
                                                            options.UseKrokhinNET);

            //DisplayScansVsNet(analysis.mstrDataset);

            analysis.mdbl_scan_net_intercept = intercept;
            analysis.mdbl_scan_net_slope = slope;
            analysis.mint_num_scans = numScans;
            analysis.mdbl_scan_net_rsquared = rsquared;

            massTagDatabase.AddResults(xTandemPHRPResultsReader, (Regressor.RegressionType)options.RegressionType, analysis);

            DateTime add = DateTime.Now;
            Console.WriteLine("# of items = " + xTandemPHRPResultsReader.marrXTandemResults.Length + " Read Time = " + Convert.ToString(read.Subtract(now)) + " Insert time = " + Convert.ToString(add.Subtract(read)));
        }		
        #endregion
    }
}
