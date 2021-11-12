using System;
using System.Collections.Generic;
using System.IO;
using MTDBFramework.Data;
using MTDBFrameworkBase.Data;
using PHRPReader.Data;
using PHRPReader.Reader;

namespace MTDBFramework.IO
{
    /// <summary>
    /// Summary Description for MSGFPlusPHRP Reader
    /// </summary>
    public class MsgfPlusPhrpReader : PHRPReaderBase
    {
        /// <summary>
        /// Configure the options for MSGFPlusReader
        /// </summary>
        /// <param name="options"></param>
        public MsgfPlusPhrpReader(Options options)
        {
            ReaderOptions = options;
        }

        /// <summary>
        /// Read and process a MSGF+ PHRP file
        /// </summary>
        /// <param name="path">MSGF+ file to read</param>
        /// <returns></returns>
        public override LcmsDataSet Read(string path)
        {
            var results = new List<MsgfPlusResult>();
            var filter = new MsgfPlusTargetFilter(ReaderOptions);

            // Get the Evidences using PHRPReader which looks at the path that was passed in to determine the data type
            var resultsProcessed = 0;
            var reader = InitializeReader(path);

            while (reader.MoveNext())
            {
                resultsProcessed++;
                if (resultsProcessed % 500 == 0)
                    UpdateProgress(reader.PercentComplete);

                if (AbortRequested)
                    break;

                var currentPSM = reader.CurrentPSM;

                // Skip this PSM if it doesn't pass the import filters
                // Note that qValue is basically FDR
                var qValue = currentPSM.GetScoreDbl(MSGFPlusSynFileReader.GetColumnNameByID(MSGFPlusSynFileColumns.QValue), -1);
                if (qValue < 0)
                    qValue = currentPSM.GetScoreDbl(MSGFPlusSynFileReader.GetMSGFDBColumnNameByID(MSGFDBSynFileColumns.FDR), 0);

                double specProb = 0;
                if (!string.IsNullOrEmpty(currentPSM.MSGFSpecEValue))
                    specProb = Convert.ToDouble(currentPSM.MSGFSpecEValue);

                if (filter.ShouldFilter(qValue, specProb))
                    continue;

                reader.FinalizeCurrentPSM();

                if (currentPSM.SeqID == 0)
                    continue;

                var result = new MsgfPlusResult
                {
                    AnalysisId = currentPSM.ResultID
                };

                StorePsmData(result, reader, specProb);

                StoreDatasetInfo(result, reader, path);
                result.DataSet.Tool = LcmsIdentificationTool.MsgfPlus;

                // Populate items specific to MGSF+
                result.Reference = currentPSM.ProteinFirst;
                result.NumTrypticEnds = currentPSM.NumTrypticTermini;

                result.DeNovoScore = currentPSM.GetScoreInt(MSGFPlusSynFileReader.GetColumnNameByID(MSGFPlusSynFileColumns.DeNovoScore));
                result.MsgfScore = currentPSM.GetScoreInt(MSGFPlusSynFileReader.GetColumnNameByID(MSGFPlusSynFileColumns.MSGFScore), 0);

                result.SpecEValue = currentPSM.GetScoreDbl(MSGFPlusSynFileReader.GetColumnNameByID(MSGFPlusSynFileColumns.SpecEValue), -1);
                if (result.SpecEValue < 0)
                {
                    result.SpecEValue = currentPSM.GetScoreDbl(MSGFPlusSynFileReader.GetMSGFDBColumnNameByID(MSGFDBSynFileColumns.SpecProb), 0);
                    result.RankSpecEValue = currentPSM.GetScoreInt(MSGFPlusSynFileReader.GetMSGFDBColumnNameByID(MSGFDBSynFileColumns.RankSpecProb), 0);
                }
                else
                {
                    result.RankSpecEValue = currentPSM.GetScoreInt(MSGFPlusSynFileReader.GetColumnNameByID(MSGFPlusSynFileColumns.SpecEValue), 0);
                }

                result.EValue = currentPSM.GetScoreDbl(MSGFPlusSynFileReader.GetColumnNameByID(MSGFPlusSynFileColumns.EValue), 0);

                result.QValue = qValue;
                result.DiscriminantValue = qValue;

                result.PepQValue = currentPSM.GetScoreDbl(MSGFPlusSynFileReader.GetColumnNameByID(MSGFPlusSynFileColumns.PepQValue), -1);
                if (result.PepQValue < 0)
                    result.PepQValue = currentPSM.GetScoreDbl(MSGFPlusSynFileReader.GetMSGFDBColumnNameByID(MSGFDBSynFileColumns.PepFDR), 0);

                result.IsotopeError = currentPSM.GetScoreInt(MSGFPlusSynFileReader.GetColumnNameByID(MSGFPlusSynFileColumns.IsotopeError), 0);

                results.Add(result);
            }

            // Calculate the Normalized Elution Times
            ComputeNETs(results);

            UpdateProgress(PROGRESS_PCT_COMPLETE, "Loading complete");

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MsgfPlus, results);
        }
    }
}
