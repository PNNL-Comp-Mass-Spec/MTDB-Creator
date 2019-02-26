#region Namespaces

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;
using MTDBFrameworkBase.Data;

#endregion

namespace MTDBFramework.IO
{
    /// <summary>
    /// Peptide Cache
    /// </summary>
    public static class PeptideCache
    {
        private static readonly ConcurrentDictionary<string, double> Cache;

        static PeptideCache()
        {
            var numProcessors = Environment.ProcessorCount;
            var concurrencyLevel = numProcessors * 2;
            const int initialCapacity = 10000;

            Cache = new ConcurrentDictionary<string, double>(concurrencyLevel, initialCapacity);
        }

        /// <summary>
        /// Clear the peptide cache
        /// </summary>
        public static void Clear()
        {
            Cache.Clear();
        }

        /// <summary>
        /// Query for existence of a peptide in the cache
        /// </summary>
        /// <param name="peptide"></param>
        /// <returns></returns>
        [Obsolete("Use TryGetValue")]
        public static bool HasValue(string peptide)
        {
            return Cache.ContainsKey(peptide);
        }

        /// <summary>
        /// If the peptide exists in the cache, returns its predictedNET
        /// </summary>
        /// <param name="peptide"></param>
        /// <param name="predictedNET"></param>
        /// <returns>True if successful, false otherwise</returns>
        public static bool TryGetValue(string peptide, out double predictedNET)
        {
            return Cache.TryGetValue(peptide, out predictedNET);
        }

        /// <summary>
        /// Add a peptide and its NET to the cache
        /// </summary>
        /// <param name="peptide"></param>
        /// <param name="net"></param>
        public static void Add(string peptide, double net)
        {
            Cache.TryAdd(peptide, net);
        }

        /// <summary>
        /// Retrieves the predictedNET of peptide from the cache
        /// </summary>
        /// <param name="peptide"></param>
        /// <returns></returns>
        [Obsolete("Use TryGetValue")]
        public static double RetrieveValue(string peptide)
        {
            if (TryGetValue(peptide, out var predictedNET))
                return predictedNET;

            return 0;

        }
    }

    /// <summary>
    /// Analysis Reader Helper
    /// </summary>
    public class AnalysisReaderHelper
    {

        //private int mPeptidesProcessed;
        //private int mPeptidesToProcess;

        /// <summary>
        /// Calculate the observed NET values
        /// </summary>
        /// <param name="evidences"></param>
        /// <remarks>Will use the MASIC scanstats.txt file or DeconTools scans.csv file if they are available</remarks>
        public void CalculateObservedNet(IEnumerable<Evidence> evidences)
        {
            // Convert to a list to suppress the "Possible multiple enumeration" warning
            var lstEvidences = evidences.ToList();
            if (lstEvidences.Count != 0)
            {
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
                    var scanToTime = ReadScanTimeFile(txtPath, 1, 2, '\t');

                    ConvertElutionTimeToNET(lstEvidences, scanToTime);

                }
                else if (File.Exists(csvPath))
                {
                    // Obtain the elution time info from the DeconTools _scans.csv file
                    // Column index 0 is scan number and index 1 is elution time
                    var scanToTime = ReadScanTimeFile(csvPath, 0, 1, ',');

                    ConvertElutionTimeToNET(lstEvidences, scanToTime);
                }
                else
                {
                    // Base elution time on the min and max scan
                    double maxScan = lstEvidences.Max(result => result.Scan);
                    double minScan = lstEvidences.Min(result => result.Scan);

                    foreach (var evidence in lstEvidences)
                    {
                        evidence.ObservedNet = (evidence.Scan - minScan) / (maxScan - minScan);
                    }
                }
            }
        }

        private void ConvertElutionTimeToNET(IEnumerable<Evidence> evidences, Dictionary<int, double> scanToTime)
        {
            var maxTime = scanToTime.Max(scanTimePair => scanTimePair.Value);
            var minTime = scanToTime.Min(scanTimePair => scanTimePair.Value);

            var lstScans = scanToTime.Keys.ToList();
            lstScans.Sort();

            foreach (var evidence in evidences)
            {
                if (!scanToTime.TryGetValue(evidence.Scan, out var observedTime))
                {
                    // Exact match not found; find the closest match
                    var index = lstScans.BinarySearch(evidence.Scan);

                    if (index < 0)
                    {
                        var indexSmaller = ~index - 1;

                        if (indexSmaller < 0)
                            indexSmaller = 0;

                        double timeEnd;

                        scanToTime.TryGetValue(lstScans[indexSmaller], out var timeStart);

                        if (indexSmaller < lstScans.Count - 1)
                            scanToTime.TryGetValue(lstScans[indexSmaller + 1], out timeEnd);
                        else
                            timeEnd = timeStart;

                        observedTime = (timeStart + timeEnd) / 2;
                    }
                    else
                    {
                        scanToTime.TryGetValue(lstScans[index], out observedTime);
                    }


                }

                evidence.ObservedNet = (observedTime - minTime) / (maxTime - minTime);
            }
        }

        private Dictionary<int, double> ReadScanTimeFile(string txtPath, int scanColIndex, int timeColIndex, char delimiter)
        {
            var scanToTime = new Dictionary<int, double>();
            var minimumColCount = Math.Max(scanColIndex, timeColIndex) + 1;

            using (var reader = new StreamReader(txtPath))
            {

                while (reader.Peek() > -1)
                {
                    var line = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    var parsedLine = line.Split(delimiter);

                    if (parsedLine.Length < minimumColCount)
                        continue;

                    if (!int.TryParse(parsedLine[scanColIndex], out var scanNumber))
                        continue;

                    if (!double.TryParse(parsedLine[timeColIndex], out var elutionTimeMinutes))
                        continue;

                    scanToTime.Add(scanNumber, elutionTimeMinutes);

                }
            }

            return scanToTime;
        }

        /// <summary>
        /// Calculate the predicted NETs of the evidences
        /// </summary>
        /// <param name="predictor">Retention Time Predictor</param>
        /// <param name="evidences">Evidences to process</param>
        /// <remarks>
        /// For each evidence, it passes the clean peptide sequence through the peptideCache alongside
        /// the predictor to determine the predicted NET. If the peptide has been seen before, it has
        /// already been added into a dictionary and so it simply looks up the relevant NET for the
        /// peptide and returns that. Otherwise, it passes the peptide through the predictor's
        /// GetElutionTime method, adds that value to the peptide cache with the sequence as the key
        /// so that if it is seen again it will get the value faster.
        /// </remarks>
        public void CalculatePredictedNet(IRetentionTimePredictor predictor, IEnumerable<Evidence> evidences)
        {
            Parallel.ForEach(evidences, evidence =>
            {
                evidence.PredictedNet = ComputePeptideNET(evidence.CleanPeptide, predictor);
            });

        }

        /// <summary>
        /// Calculate the predicted NET of a single peptide
        /// </summary>
        /// <param name="peptide"></param>
        /// <param name="predictor">Retention Time Predictor</param>
        /// <returns></returns>
        public double ComputePeptideNET(string peptide, IRetentionTimePredictor predictor)
        {
            if (!PeptideCache.TryGetValue(peptide, out var predictedNET))
            {
                predictedNET = predictor.GetElutionTime(peptide);
                PeptideCache.Add(peptide, predictedNET);
            }

            return predictedNET;
        }

    }


}