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
        /// Read and process a SEQUEST PHRP file
        /// </summary>
        /// <param name="path">SEQUEST file to read</param>
        /// <returns></returns>
        public override LcmsDataSet Read(string path)
        {
            var results = new List<SequestResult>();
            var filter = new SequestTargetFilter(ReaderOptions);

            // Get the Evidences using PHRPReader which looks at the path that was passed in to determine the data type
            var resultsProcessed = 0;
            var reader = InitializeReader(path);

            while (reader.MoveNext())
            {
                resultsProcessed++;
                if (resultsProcessed % 500 == 0)
                    UpdateProgress(reader.PercentComplete, "Reading peptides");

                if (AbortRequested)
                    break;

                var currentPSM = reader.CurrentPSM;

                // Skip this PSM if it doesn't pass the import filters
                var xcorr = currentPSM.GetScoreDbl(SequestSynFileReader.GetColumnNameByID(SequestSynopsisFileColumns.XCorr), 0);

                double specProb = 0;
                if (!string.IsNullOrEmpty(currentPSM.MSGFSpecEValue))
                    specProb = Convert.ToDouble(currentPSM.MSGFSpecEValue);

                if (filter.ShouldFilter(xcorr, specProb))
                    continue;

                reader.FinalizeCurrentPSM();

                if (currentPSM.SeqID == 0)
                    continue;

                var result = new SequestResult
                {
                    AnalysisId = currentPSM.ResultID
                };

                StorePsmData(result, reader, specProb);

                StoreDatasetInfo(result, reader, path);

                // Populate items specific to SEQUEST
                result.Reference = currentPSM.ProteinFirst;
                result.NumTrypticEnds = currentPSM.NumTrypticTermini;

                result.DelCn = currentPSM.GetScoreDbl(SequestSynFileReader.GetColumnNameByID(SequestSynopsisFileColumns.DeltaCn), 0);
                result.DelCn2 = currentPSM.GetScoreDbl(SequestSynFileReader.GetColumnNameByID(SequestSynopsisFileColumns.DeltaCn2), 0);

                result.RankSp = (short)currentPSM.GetScoreInt(SequestSynFileReader.GetColumnNameByID(SequestSynopsisFileColumns.RankSP), 0);
                result.RankXc = (short)currentPSM.GetScoreInt(SequestSynFileReader.GetColumnNameByID(SequestSynopsisFileColumns.RankXC), 0);
                result.Sp = currentPSM.GetScoreDbl(SequestSynFileReader.GetColumnNameByID(SequestSynopsisFileColumns.Sp), 0);
                result.XCorr = xcorr;
                result.DiscriminantValue = xcorr;
                result.XcRatio = currentPSM.GetScoreDbl(SequestSynFileReader.GetColumnNameByID(SequestSynopsisFileColumns.XcRatio), 0);

                result.FScore = SequestResult.CalculatePeptideProphetDiscriminantScore(result);

                results.Add(result);
            }

            // Calculate the Normalized Elution Times
            ComputeNETs(results);

            UpdateProgress(PROGRESS_PCT_COMPLETE, "Loading complete");

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.Sequest, results);
        }
    }
}
