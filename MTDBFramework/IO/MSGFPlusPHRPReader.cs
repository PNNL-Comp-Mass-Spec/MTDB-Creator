using System;
using System.Collections.Generic;
using System.IO;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFramework.Database;
using PHRPReader;

namespace MTDBFramework.IO
{
    /// <summary>
    /// Summary Description for MSGFPlusPHRP Reader
    /// </summary>
    public class MsgfPlusPhrpReader : PHRPReaderBase
    {
        public MsgfPlusPhrpReader(Options options)
        {
            ReaderOptions = options;
        }

        public override LcmsDataSet Read(string path)
        {
            var results = new List<MsgfPlusResult>();
            var filter = new MsgfPlusTargetFilter(ReaderOptions);

            // Get the Evidences using PHRPReader which looks at the path that was passed in to determine the data type
            int resultsProcessed = 0;
            var reader = InitializeReader(path);

            while (reader.MoveNext())
            {
                resultsProcessed++;
                if (resultsProcessed % 500 == 0)
                    UpdateProgress(reader.PercentComplete);

                if (mAbortRequested)
                    break;

                // Skip this PSM if it doesn't pass the import filters
                // Note that qValue is basically FDR
                double qValue = reader.CurrentPSM.GetScoreDbl(clsPHRPParserMSGFDB.DATA_COLUMN_QValue, -1);
                if (qValue < 0)
                    qValue = reader.CurrentPSM.GetScoreDbl(clsPHRPParserMSGFDB.DATA_COLUMN_FDR, 0);

                double specProb = 0;
                if (!string.IsNullOrEmpty(reader.CurrentPSM.MSGFSpecProb))
                    specProb = Convert.ToDouble(reader.CurrentPSM.MSGFSpecProb);

                if (filter.ShouldFilter(qValue, specProb))
                    continue;

                reader.FinalizeCurrentPSM();

                if (reader.CurrentPSM.SeqID == 0)
                    continue;

                var result = new MsgfPlusResult
                {
                    AnalysisId = reader.CurrentPSM.ResultID
                };

                StorePSMData(result, reader, specProb);

                StoreDatasetInfo(result, reader, path);

                // Populate items specific to MGSF+
                result.Reference = reader.CurrentPSM.ProteinFirst;
                result.NumTrypticEnds = reader.CurrentPSM.NumTrypticTerminii;

                result.DeNovoScore = reader.CurrentPSM.GetScoreInt(clsPHRPParserMSGFDB.DATA_COLUMN_DeNovoScore, 0);
                result.MsgfScore = reader.CurrentPSM.GetScoreInt(clsPHRPParserMSGFDB.DATA_COLUMN_MSGFScore, 0);

                result.SpecEValue = reader.CurrentPSM.GetScoreDbl(clsPHRPParserMSGFDB.DATA_COLUMN_MSGFDB_SpecEValue, -1);
                if (result.SpecEValue < 0)
                {
                    result.SpecEValue = reader.CurrentPSM.GetScoreDbl(clsPHRPParserMSGFDB.DATA_COLUMN_MSGFDB_SpecProb, 0);
                    result.RankSpecEValue = reader.CurrentPSM.GetScoreInt(clsPHRPParserMSGFDB.DATA_COLUMN_Rank_MSGFDB_SpecProb, 0);
                }
                else
                {
                    result.RankSpecEValue = reader.CurrentPSM.GetScoreInt(clsPHRPParserMSGFDB.DATA_COLUMN_Rank_MSGFDB_SpecEValue, 0);
                }

                result.EValue = reader.CurrentPSM.GetScoreDbl(clsPHRPParserMSGFDB.DATA_COLUMN_EValue, 0);

                result.QValue = qValue;
                result.PepQValue = reader.CurrentPSM.GetScoreDbl(clsPHRPParserMSGFDB.DATA_COLUMN_PepQValue, -1);
                if (result.PepQValue < 0)
                    result.PepQValue = reader.CurrentPSM.GetScoreDbl(clsPHRPParserMSGFDB.DATA_COLUMN_PepFDR, 0);

                result.IsotopeError = reader.CurrentPSM.GetScoreInt(clsPHRPParserMSGFDB.DATA_COLUMN_Isotope_Error, 0);

                results.Add(result);
            }

            ComputeNETs(results);

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MsgfPlus, results);
        }

    }
}
