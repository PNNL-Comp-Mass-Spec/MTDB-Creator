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
    /// Summary Description for XTandemPHRP Reader
    /// </summary>
    public class XTandemPhrpReader : PHRPReaderBase
    {
        /// <summary>
        /// Configure the options for XTandemPhrpReader
        /// </summary>
        /// <param name="options"></param>
        public XTandemPhrpReader(Options options)
        {
            ReaderOptions = options;
        }

        /// <summary>
        /// Read and process a XTandem PHRP file
        /// </summary>
        /// <param name="path">XTandem file to read</param>
        /// <returns></returns>
        public override LcmsDataSet Read(string path)
        {
            var results = new List<XTandemResult>();
            var filter = new XTandemTargetFilter(ReaderOptions);

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
                var logPepEValue = currentPSM.GetScoreDbl(XTandemSynFileReader.GetColumnNameByID(XTandemSynFileColumns.EValue), 0);

                double specProb = 0;
                if (!string.IsNullOrEmpty(currentPSM.MSGFSpecEValue))
                    specProb = Convert.ToDouble(currentPSM.MSGFSpecEValue);

                if (filter.ShouldFilter(logPepEValue, specProb))
                    continue;

                reader.FinalizeCurrentPSM();

                if (currentPSM.SeqID == 0)
                    continue;

                var result = new XTandemResult
                {
                    AnalysisId = currentPSM.ResultID
                };

                StorePsmData(result, reader, specProb);

                StoreDatasetInfo(result, reader, path);
                result.DataSet.Tool = LcmsIdentificationTool.XTandem;

                // Populate items specific to X!Tandem
                result.NumTrypticEnds = currentPSM.NumTrypticTermini;

                result.BScore = currentPSM.GetScoreDbl(XTandemSynFileReader.GetColumnNameByID(XTandemSynFileColumns.BScore), 0);
                result.DeltaCn2 = currentPSM.GetScoreDbl(XTandemSynFileReader.GetColumnNameByID(XTandemSynFileColumns.DeltaCn2), 0);
                result.LogIntensity = currentPSM.GetScoreDbl(XTandemSynFileReader.GetColumnNameByID(XTandemSynFileColumns.Intensity), 0);
                result.LogPeptideEValue = logPepEValue;
                result.DiscriminantValue = logPepEValue;
                result.NumberBIons = (short)currentPSM.GetScoreInt(XTandemSynFileReader.GetColumnNameByID(XTandemSynFileColumns.BIons));
                result.NumberYIons = (short)currentPSM.GetScoreInt(XTandemSynFileReader.GetColumnNameByID(XTandemSynFileColumns.YIons));
                result.PeptideHyperscore = currentPSM.GetScoreDbl(XTandemSynFileReader.GetColumnNameByID(XTandemSynFileColumns.Hyperscore), 0);
                result.YScore = currentPSM.GetScoreDbl(XTandemSynFileReader.GetColumnNameByID(XTandemSynFileColumns.YScore), 0);

                results.Add(result);
            }

            // Calculate the Normalized Elution Times
            ComputeNETs(results);

            UpdateProgress(PROGRESS_PCT_COMPLETE, "Loading complete");

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.XTandem, results);
        }
    }
}
