﻿using System;
using System.Collections.Generic;
using System.IO;
using MTDBFramework.Data;
using MTDBFrameworkBase.Data;
using PHRPReader.Data;
using PHRPReader.Reader;

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
                var eValue = currentPSM.GetScoreDbl(MSAlignSynFileReader.GetColumnNameByID(MSAlignSynFileColumns.EValue), 0);

                double specProb = 0;
                if (!string.IsNullOrEmpty(currentPSM.MSGFSpecEValue))
                    specProb = Convert.ToDouble(currentPSM.MSGFSpecEValue);

                if (filter.ShouldFilter(eValue, specProb))
                    continue;

                reader.FinalizeCurrentPSM();

                if (currentPSM.ResultID == 0)
                    continue;

                var result = new MsAlignResult
                {
                    AnalysisId = currentPSM.ResultID
                };

                StorePsmData(result, reader, specProb);

                StoreDatasetInfo(result, reader, path);
                result.DataSet.Tool = LcmsIdentificationTool.MSAlign;

                // Populate items specific to MSAlign
                result.EValue = eValue;
                result.DiscriminantValue = eValue;

                results.Add(result);
            }

            // Calculate the Normalized Elution Times
            ComputeNETs(results);

            UpdateProgress(PROGRESS_PCT_COMPLETE, "Loading complete");

            return new LcmsDataSet(Path.GetFileNameWithoutExtension(path), LcmsIdentificationTool.MSAlign, results);
        }
    }
}
