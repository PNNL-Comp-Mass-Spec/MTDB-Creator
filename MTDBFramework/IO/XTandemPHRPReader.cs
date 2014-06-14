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
    /// Summary Description for XTandemPHRP Reader
    /// </summary>
    public class XTandemPhrpReader : PHRPReaderBase
    {
        public XTandemPhrpReader(Options options)
        {
            ReaderOptions = options;
        }

        public override LcmsDataSet Read(string path)
        {
            var results = new List<XTandemResult>();
            var filter = new XTandemTargetFilter(ReaderOptions);

            int resultsProcessed = 0;

            // Get the Evidences using PHRPReader which looks at the path that was passed in
            var reader = new clsPHRPReader(path);
            while (reader.CanRead)
            {                
                reader.MoveNext();

                resultsProcessed++;
                if (resultsProcessed % 500 == 0)
                    UpdateProgress(reader.PercentComplete);

                if (reader.CurrentPSM.SeqID == 0)
                    continue;

                // Skip this PSM if it doesn't pass the import filters
                double logPepEValue = reader.CurrentPSM.GetScoreDbl(clsPHRPParserXTandem.DATA_COLUMN_Peptide_Expectation_Value_LogE, 0);
                
                double specProb = 0;
                if (!string.IsNullOrEmpty(reader.CurrentPSM.MSGFSpecProb))
                    specProb = Convert.ToDouble(reader.CurrentPSM.MSGFSpecProb);

                if (filter.ShouldFilter(logPepEValue, specProb))
                    continue;

                var result = new XTandemResult
                {
                    AnalysisId = reader.CurrentPSM.ResultID                    
                };

                StorePSMData(result, reader, specProb);

                StoreDatasetInfo(result, reader, path);


                // Populate items specific to X!Tandem
                
                result.NumTrypticEnds = reader.CurrentPSM.NumTrypticTerminii;


                result.BScore = reader.CurrentPSM.GetScoreDbl(clsPHRPParserXTandem.DATA_COLUMN_b_score, 0);
                result.DeltaCn2 = reader.CurrentPSM.GetScoreDbl(clsPHRPParserXTandem.DATA_COLUMN_DeltaCn2, 0);                
                result.LogIntensity = reader.CurrentPSM.GetScoreDbl(clsPHRPParserXTandem.DATA_COLUMN_Peptide_Intensity_LogI, 0);
                result.LogPeptideEValue = logPepEValue;
                result.NumberBIons = (short)reader.CurrentPSM.GetScoreInt(clsPHRPParserXTandem.DATA_COLUMN_b_ions);
                result.NumberYIons = (short)reader.CurrentPSM.GetScoreInt(clsPHRPParserXTandem.DATA_COLUMN_y_ions);
                result.PeptideHyperscore = reader.CurrentPSM.GetScoreDbl(clsPHRPParserXTandem.DATA_COLUMN_Peptide_Hyperscore, 0);
                result.YScore = reader.CurrentPSM.GetScoreDbl(clsPHRPParserXTandem.DATA_COLUMN_y_score, 0);

                results.Add(result);
            }

            AnalysisReaderHelper.CalculateObservedNet(results);
            AnalysisReaderHelper.CalculatePredictedNet(RetentionTimePredictorFactory.CreatePredictor(ReaderOptions.PredictorType), results);
            
            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.XTandem, results);
        }
    }  
    
}
