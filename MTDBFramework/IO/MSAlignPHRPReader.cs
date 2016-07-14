using System;
using System.Collections.Generic;
using System.IO;
using MTDBFramework.Data;
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

            // Get the Evidences using PHRPReader which looks at the path that was passed in to determine the data type
            int resultsProcessed = 0;
            var reader = InitializeReader(path);

            while (reader.MoveNext())
            {
                resultsProcessed++;
                if (resultsProcessed % 500 == 0)
                    UpdateProgress(reader.PercentComplete);

                if (AbortRequested)
                    break;

                // Skip this PSM if it doesn't pass the import filters
                double eValue = reader.CurrentPSM.GetScoreDbl(clsPHRPParserMSAlign.DATA_COLUMN_EValue, 0);

                double specProb = 0;
                if (!string.IsNullOrEmpty(reader.CurrentPSM.MSGFSpecProb))
                    specProb = Convert.ToDouble(reader.CurrentPSM.MSGFSpecProb);

                if (filter.ShouldFilter(eValue, specProb))
                    continue;

                reader.FinalizeCurrentPSM();

                if (reader.CurrentPSM.ResultID == 0)
                    continue;

                var result = new MsAlignResult
                {
                    AnalysisId = reader.CurrentPSM.ResultID
                };

                StorePsmData(result, reader, specProb);

                StoreDatasetInfo(result, reader, path);
                result.DataSet.Tool = LcmsIdentificationTool.MSAlign;

                // Populate items specific to MSAlign
                result.EValue = eValue;
                result.DiscriminantValue = eValue;

                results.Add(result);
            }

            ComputeNets(results);

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MSAlign, results);
        }

    }
}
