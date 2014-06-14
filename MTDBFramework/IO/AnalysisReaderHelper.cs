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

        [Obsolete("Use TryGetValue")]
        public static bool HasValue(string peptide)
        {
            return Cache.ContainsKey(peptide);
        }

        public static bool TryGetValue(string peptide, out double predictedNET)
        {
            return Cache.TryGetValue(peptide, out predictedNET);            
        }

        public static void Add(string peptide, double net)
        {
            Cache.Add(peptide, net);
        }

        [Obsolete("Use TryGetValue")]
        public static double RetrieveValue(string peptide)
        {
            return Cache[peptide];
        }
    }

    public class CacheAccessor
    {
        public double PredictPeptide(string peptide, IRetentionTimePredictor predictor)
        {
            double predictedNET;
            if (!PeptideCache.TryGetValue(peptide, out predictedNET))
            {
                predictedNET = predictor.GetElutionTime(peptide);
                PeptideCache.Add(peptide, predictedNET);
            }

            return predictedNET;
        }
    }

    public static class AnalysisReaderHelper
    {
        /// <summary>
        /// Calculate the observed NET values
        /// </summary>
        /// <param name="evidences"></param>
        /// <remarks>Will use the MASIC scanstats.txt file or DeconTools scans.csv file if they are available</remarks>
        public static void CalculateObservedNet(IEnumerable<Evidence> evidences)
        {
            // Convert to a list to suppress the "Possible multiple enumeration" warning
            var lstEvidences = evidences.ToList();

            var dataset = lstEvidences.First().DataSet;

            var fiDataset = new FileInfo(dataset.Path);

            if (fiDataset.Directory == null)
            {
                // Invalid directory
                return;
            }

            var jobFolder = fiDataset.Directory.FullName;
            var csvPath = Path.Combine(jobFolder, dataset.Name + "_scans.csv");
            var txtPath = Path.Combine(jobFolder, dataset.Name + "_scanstats.txt");

            if (File.Exists(txtPath))
            {
                // Obtain the elution time info from the Masic _scanstats.txt file
                // Column index 1 is scan number and index 2 is elution time
                var scanToTime = ReadScanTimeFile(txtPath, 1, 2);

                AssignNETs(lstEvidences, scanToTime);

            }
            else if (File.Exists(csvPath))
            {
                // Obtain the elution time info from the DeconTools _scans.csv file
                // Column index 0 is scan number and index 1 is elution time
                var scanToTime = ReadScanTimeFile(txtPath, 0, 1);

                AssignNETs(lstEvidences, scanToTime);
            }
            else
            {
                // Base elution time on the min and max scan
                double maxScan = lstEvidences.Max(result => result.Scan);
                double minScan = lstEvidences.Min(result => result.Scan);

                foreach (var evidence in lstEvidences)
                {
                    evidence.ObservedNet = (evidence.Scan - minScan)/(maxScan - minScan);
                }
            }
        }

        private static void AssignNETs(IEnumerable<Evidence> evidences, Dictionary<int, double> scanToTime)
        {
            var maxTime = scanToTime.Max(scanTimePair => scanTimePair.Value);
            var minTime = scanToTime.Min(scanTimePair => scanTimePair.Value);

            foreach (var evidence in evidences)
            {
                int scanStart = 1;
                int scanEnd = 1;
                double timeStart = minTime;
                double timeEnd = maxTime;
                bool exactMatch = false;
                double observedTime;

                foreach (var scanTimePair in scanToTime)
                {
                    if (evidence.Scan == scanTimePair.Key)
                    {
                        exactMatch = true;
                        timeEnd = scanTimePair.Value;
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
                    observedTime = ((double)(evidence.Scan - scanStart) / (scanEnd - scanStart)) *
                                   (timeEnd - timeStart) + timeStart;
                }

                evidence.ObservedNet = (observedTime - minTime) / (maxTime - minTime);
            }
        }

        private static Dictionary<int, double> ReadScanTimeFile(string txtPath, int scanColIndex, int timeColIndex)
        {
            var scanToTime = new Dictionary<int, double>();
            int minimumColCount = Math.Max(scanColIndex, timeColIndex) + 1;

            using (var reader = new StreamReader(txtPath))
            {

                while (reader.Peek() > -1)
                {
                    var line = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var parsedLine = line.Split('\t');

                    if (parsedLine.Length < minimumColCount)
                        continue;

                    int scanNumber;
                    if (!int.TryParse(parsedLine[scanColIndex], out scanNumber))
                        continue;

                    double elutionTimeMinutes;
                    if (!double.TryParse(parsedLine[timeColIndex], out elutionTimeMinutes))
                        continue;

                    scanToTime.Add(scanNumber, elutionTimeMinutes);
                    
                }
            }

            return scanToTime;
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
                
            }
        }
    }
}