using System;
using System.Collections.Generic;
using System.IO;
using MTDBFramework.Data;
using PHRPReader;

namespace MTDBFramework.IO
{
    /// <summary>
    /// Summary Description for SequestPHRP Reader
    /// </summary>
    public class SequestPhrpReader : PHRPReaderBase
    {
        /// <summary>
        /// Configure the options for SequestReader
        /// </summary>
        /// <param name="options"></param>
        public SequestPhrpReader(Options options)
        {
            ReaderOptions = options;
        }

        /// <summary>
        /// Read and process a Sequest PHRP file
        /// </summary>
        /// <param name="path">Sequest file to read</param>
        /// <returns></returns>
        public override LcmsDataSet Read(string path)
        {
            var results = new List<SequestResult>();
            var filter = new SequestTargetFilter(ReaderOptions);

            // Get the Evidences using PHRPReader which looks at the path that was passed in to determine the data type
            int resultsProcessed = 0;
            var reader = InitializeReader(path);

            while (reader.MoveNext())
            {
                resultsProcessed++;
                if (resultsProcessed % 500 == 0)
                    UpdateProgress(reader.PercentComplete, "Reading peptides");

                if (AbortRequested)
                    break;

                // Skip this PSM if it doesn't pass the import filters
                double xcorr = reader.CurrentPSM.GetScoreDbl(clsPHRPParserSequest.DATA_COLUMN_XCorr, 0);

                double specProb = 0;
                if (!string.IsNullOrEmpty(reader.CurrentPSM.MSGFSpecEValue))
                    specProb = Convert.ToDouble(reader.CurrentPSM.MSGFSpecEValue);

                if (filter.ShouldFilter(xcorr, specProb))
                    continue;

                reader.FinalizeCurrentPSM();

                if (reader.CurrentPSM.SeqID == 0)
                    continue;

                var result = new SequestResult
                {
                    AnalysisId = reader.CurrentPSM.ResultID
                };

                StorePsmData(result, reader, specProb);

                StoreDatasetInfo(result, reader, path);

                // Populate items specific to Sequest
                result.Reference = reader.CurrentPSM.ProteinFirst;
                result.NumTrypticEnds = reader.CurrentPSM.NumTrypticTerminii;

                result.DelCn = reader.CurrentPSM.GetScoreDbl(clsPHRPParserSequest.DATA_COLUMN_DelCn, 0);
                result.DelCn2 = reader.CurrentPSM.GetScoreDbl(clsPHRPParserSequest.DATA_COLUMN_DelCn2, 0);

                result.RankSp = (short)reader.CurrentPSM.GetScoreInt(clsPHRPParserSequest.DATA_COLUMN_RankSp, 0);
                result.RankXc = (short)reader.CurrentPSM.GetScoreInt(clsPHRPParserSequest.DATA_COLUMN_RankXc, 0);
                result.Sp = reader.CurrentPSM.GetScoreDbl(clsPHRPParserSequest.DATA_COLUMN_Sp, 0);
                result.XCorr = xcorr;
                result.DiscriminantValue = xcorr;
                result.XcRatio = reader.CurrentPSM.GetScoreDbl(clsPHRPParserSequest.DATA_COLUMN_XcRatio, 0);

                result.FScore = SequestResult.CalculatePeptideProphetDiscriminantScore(result);

                results.Add(result);
            }

            ComputeNets(results);

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.Sequest, results);
        }

    }
}
