using System;
using System.Collections.Generic;
using System.IO;
using MTDBFramework.Data;
using PHRPReader;

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

                // Skip this PSM if it doesn't pass the import filters
                var logPepEValue = reader.CurrentPSM.GetScoreDbl(clsPHRPParserXTandem.DATA_COLUMN_Peptide_Expectation_Value_LogE, 0);

                double specProb = 0;
                if (!string.IsNullOrEmpty(reader.CurrentPSM.MSGFSpecEValue))
                    specProb = Convert.ToDouble(reader.CurrentPSM.MSGFSpecEValue);

                if (filter.ShouldFilter(logPepEValue, specProb))
                    continue;

                reader.FinalizeCurrentPSM();

                if (reader.CurrentPSM.SeqID == 0)
                    continue;

                var result = new XTandemResult
                {
                    AnalysisId = reader.CurrentPSM.ResultID
                };

                StorePsmData(result, reader, specProb);

                StoreDatasetInfo(result, reader, path);
                result.DataSet.Tool = LcmsIdentificationTool.XTandem;

                // Populate items specific to X!Tandem
                result.NumTrypticEnds = reader.CurrentPSM.NumTrypticTerminii;

                result.BScore = reader.CurrentPSM.GetScoreDbl(clsPHRPParserXTandem.DATA_COLUMN_b_score, 0);
                result.DeltaCn2 = reader.CurrentPSM.GetScoreDbl(clsPHRPParserXTandem.DATA_COLUMN_DeltaCn2, 0);
                result.LogIntensity = reader.CurrentPSM.GetScoreDbl(clsPHRPParserXTandem.DATA_COLUMN_Peptide_Intensity_LogI, 0);
                result.LogPeptideEValue = logPepEValue;
                result.DiscriminantValue = logPepEValue;
                result.NumberBIons = (short)reader.CurrentPSM.GetScoreInt(clsPHRPParserXTandem.DATA_COLUMN_b_ions);
                result.NumberYIons = (short)reader.CurrentPSM.GetScoreInt(clsPHRPParserXTandem.DATA_COLUMN_y_ions);
                result.PeptideHyperscore = reader.CurrentPSM.GetScoreDbl(clsPHRPParserXTandem.DATA_COLUMN_Peptide_Hyperscore, 0);
                result.YScore = reader.CurrentPSM.GetScoreDbl(clsPHRPParserXTandem.DATA_COLUMN_y_score, 0);

                results.Add(result);
            }

            // Calculate the Normalized Elution Times
            ComputeNETs(results);

            UpdateProgress(PROGRESS_PCT_COMPLETE, "Loading complete");

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.XTandem, results);
        }
    }

}
