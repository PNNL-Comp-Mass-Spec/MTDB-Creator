using System;
using System.Collections.Generic;
using System.IO;
using MTDBFramework.Data;
using MTDBFrameworkBase.Data;
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

                // Skip this PSM if it doesn't pass the import filters
                // Note that qValue is basically FDR
                var qValue = reader.CurrentPSM.GetScoreDbl(MSGFPlusSynFileReader.DATA_COLUMN_QValue, -1);
                if (qValue < 0)
                    qValue = reader.CurrentPSM.GetScoreDbl(MSGFPlusSynFileReader.DATA_COLUMN_FDR, 0);

                double specProb = 0;
                if (!string.IsNullOrEmpty(reader.CurrentPSM.MSGFSpecEValue))
                    specProb = Convert.ToDouble(reader.CurrentPSM.MSGFSpecEValue);

                if (filter.ShouldFilter(qValue, specProb))
                    continue;

                reader.FinalizeCurrentPSM();

                if (reader.CurrentPSM.SeqID == 0)
                    continue;

                var result = new MsgfPlusResult
                {
                    AnalysisId = reader.CurrentPSM.ResultID
                };

                StorePsmData(result, reader, specProb);

                StoreDatasetInfo(result, reader, path);
                result.DataSet.Tool = LcmsIdentificationTool.MsgfPlus;

                // Populate items specific to MGSF+
                result.Reference = reader.CurrentPSM.ProteinFirst;
                result.NumTrypticEnds = reader.CurrentPSM.NumTrypticTermini;

                result.DeNovoScore = reader.CurrentPSM.GetScoreInt(MSGFPlusSynFileReader.DATA_COLUMN_DeNovoScore, 0);
                result.MsgfScore = reader.CurrentPSM.GetScoreInt(MSGFPlusSynFileReader.DATA_COLUMN_MSGFScore, 0);

                result.SpecEValue = reader.CurrentPSM.GetScoreDbl(MSGFPlusSynFileReader.DATA_COLUMN_MSGFPlus_SpecEValue, -1);
                if (result.SpecEValue < 0)
                {
                    result.SpecEValue = reader.CurrentPSM.GetScoreDbl(MSGFPlusSynFileReader.DATA_COLUMN_MSGFDB_SpecProb, 0);
                    result.RankSpecEValue = reader.CurrentPSM.GetScoreInt(MSGFPlusSynFileReader.DATA_COLUMN_Rank_MSGFDB_SpecProb, 0);
                }
                else
                {
                    result.RankSpecEValue = reader.CurrentPSM.GetScoreInt(MSGFPlusSynFileReader.DATA_COLUMN_MSGFPlus_SpecEValue, 0);
                }

                result.EValue = reader.CurrentPSM.GetScoreDbl(MSGFPlusSynFileReader.DATA_COLUMN_EValue, 0);

                result.QValue = qValue;
                result.DiscriminantValue = qValue;

                result.PepQValue = reader.CurrentPSM.GetScoreDbl(MSGFPlusSynFileReader.DATA_COLUMN_PepQValue, -1);
                if (result.PepQValue < 0)
                    result.PepQValue = reader.CurrentPSM.GetScoreDbl(MSGFPlusSynFileReader.DATA_COLUMN_PepFDR, 0);

                result.IsotopeError = reader.CurrentPSM.GetScoreInt(MSGFPlusSynFileReader.DATA_COLUMN_Isotope_Error, 0);

                results.Add(result);
            }

            // Calculate the Normalized Elution Times
            ComputeNETs(results);

            UpdateProgress(PROGRESS_PCT_COMPLETE, "Loading complete");

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MsgfPlus, results);
        }

    }
}
