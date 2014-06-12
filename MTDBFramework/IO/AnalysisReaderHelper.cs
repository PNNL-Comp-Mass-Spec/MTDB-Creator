#region Namespaces

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.IO
{
    public static class PeptideCache
    {
        private static readonly Dictionary<string, double> Cache;

        static PeptideCache()
        {
            Cache = new Dictionary<string, double>();
        }

        public static void Clear()
        {
            Cache.Clear();
        }

        public static bool HasValue(string peptide)
        {
            return Cache.ContainsKey(peptide);
        }

        public static void Add(string peptide, double net)
        {
            Cache.Add(peptide, net);
        }

        public static double RetrieveValue(string peptide)
        {
            return Cache[peptide];
        }
    }

    public class CacheAccessor
    {
        public double PredictPeptide(string peptide, IRetentionTimePredictor predictor)
        {
            var hasBeenPredicted = PeptideCache.HasValue(peptide);
            if (!hasBeenPredicted)
            {
                var net = predictor.GetElutionTime(peptide);
                PeptideCache.Add(peptide, net);
                return net;
            }
            return PeptideCache.RetrieveValue(peptide);
        }
    }

    public static class AnalysisReaderHelper
    {
        public static void CalculateObservedNet(IEnumerable<Evidence> evidences )
        {
            // If we have the scans file, use that to calculate the observed Net
            evidences = evidences.ToList();
            var jobFolder = Path.GetDirectoryName(evidences.First().DataSet.Path);
            var csvPath = jobFolder + "\\" + evidences.First().DataSet.Name + "_scans.csv";
            var txtPath = jobFolder + "\\" + evidences.First().DataSet.Name + "_scanstats.txt";

            if (File.Exists(txtPath))
            {
                var scanToTime = new Dictionary<int, double>();
                using (var reader = new StreamReader(txtPath))
                {
                    //Read the header
                    reader.ReadLine();
                    //Read the first line
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line != null)
                        {
                            var parsedLine = line.Split('\t');

                            scanToTime.Add(Convert.ToInt32(parsedLine[1]), Convert.ToDouble(parsedLine[2]));
                        }
                    }
                }
                foreach (var evidence in evidences)
                {
                    evidence.ObservedNet = scanToTime[evidence.Scan];
                }
                
            }


            //If it's a .csv file which holds the scans data
            else if (File.Exists(csvPath))
            {
                // Create dictionary of scans to times
                var scanToTime = new Dictionary<int, double>();
                using (var reader = new StreamReader(csvPath))
                {
                    //Read the header
                    reader.ReadLine();
                    //Read the first line
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (line != null)
                        {
                            var parsedLine = line.Split(',');

                            scanToTime.Add(Convert.ToInt32(parsedLine[0]), Convert.ToDouble(parsedLine[1]));
                        }
                    }
                }

                var maxTime = scanToTime.Max(scanTimePair => scanTimePair.Value);
                var minTime = scanToTime.Min(scanTimePair => scanTimePair.Value);

                foreach (var evidence in evidences)
                {
                    int scanStart       = 1;
                    int scanEnd         = 1;
                    double timeStart    = minTime;
                    double timeEnd      = maxTime;
                    bool exactMatch     = false;
                    double observedTime;
                    
                    foreach (var scanTimePair in scanToTime)
                    {
                        if (evidence.Scan == scanTimePair.Key)
                        {
                            exactMatch  = true;
                            timeEnd     = scanTimePair.Value;
                        }
                        if (evidence.Scan < scanTimePair.Key)
                        {
                            scanEnd = scanTimePair.Key;
                            timeEnd = scanTimePair.Value;
                            break;
                        }
                        scanStart = scanTimePair.Key;
                        timeStart = scanTimePair.Value;
                    }
                    // Find the time that it was observed. Not normalized yet.
                    if (exactMatch)
                    {
                        observedTime = timeEnd;
                    }
                    else
                    {
                        observedTime = ((double) (evidence.Scan - scanStart)/(scanEnd - scanStart))*
                                       (timeEnd - timeStart) + timeStart;
                    }

                    evidence.ObservedNet = (observedTime - minTime)/(maxTime - minTime);
                }
            }

            //Otherwise, we base it on min and max scan
            else
            {
                double maxScan = evidences.Max(result => result.Scan);
                double minScan = evidences.Min(result => result.Scan);

                foreach (var evidence in evidences)
                {
                    evidence.ObservedNet = (evidence.Scan - minScan)/(maxScan - minScan);
                }
            }
        }

		// Entry point for calculating the predicted NET.
		// Accepts a Retention time predictor and an IEnumerable of Evidences
		// For each evidence, it passes the clean peptide sequence through the peptideCache alongside
		// the predictor to determine the predicted NET. If the peptide has been seen before, it has
		// already been added into a dictionary and so it simply looks up the relevant NET for the
		// peptide and returns that. Otherwise, it passes the peptide through the predictor's
		// GetElutionTime method, adds that value to the peptide cache with the sequence as the key
		// so that if it is seen again it will get the value faster.
        public static void CalculatePredictedNet(IRetentionTimePredictor predictor, IEnumerable<Evidence> evidences)
        {
            var pepCache = new CacheAccessor();
            foreach (var evidence in evidences)
            {
                evidence.PredictedNet = pepCache.PredictPeptide(evidence.CleanPeptide, predictor);
                // Old version of predicting the NET not utilizing the peptide cache
                // evidence.PredictedNet = predictor.GetElutionTime(evidence.CleanPeptide);
            }
        }
    }
}