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
    /// Summary Description for MSAlignPHRP Reader
    /// </summary>
    class MsAlignPhrpReader : PHRPReaderBase
    {
        public MsAlignPhrpReader(Options options)
        {
            ReaderOptions = options;
        }

        public override LcmsDataSet Read(string path)
        {
            var results = new List<MsAlignResult>();
            var filter = new MsAlignTargetFilter(ReaderOptions);

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
                double eValue = reader.CurrentPSM.GetScoreDbl(clsPHRPParserMSAlign.DATA_COLUMN_EValue, 0);
              
                double specProb = 0;
                if (!string.IsNullOrEmpty(reader.CurrentPSM.MSGFSpecProb))
                    specProb = Convert.ToDouble(reader.CurrentPSM.MSGFSpecProb);

                if (filter.ShouldFilter(eValue, specProb))
                    continue;

                var result = new MsAlignResult
                {
                    AnalysisId = reader.CurrentPSM.ResultID
                };

                StorePSMData(result, reader, specProb);

                StoreDatasetInfo(result, reader, path);


                // Populate items specific to MSAlign
                result.EValue = eValue;
				              
                results.Add(result);              
            }

            AnalysisReaderHelper.CalculateObservedNet(results);
            AnalysisReaderHelper.CalculatePredictedNet(RetentionTimePredictorFactory.CreatePredictor(ReaderOptions.PredictorType), results);

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MSAlign, results);
        }
    }
}
