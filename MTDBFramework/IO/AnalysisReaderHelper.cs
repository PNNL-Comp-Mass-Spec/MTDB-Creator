#region Namespaces

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.IO
{
    public static class PeptideCache
    {
        private static readonly ConcurrentDictionary<string, double> Cache;

        static PeptideCache()
        {
            int numProcs = Environment.ProcessorCount;
            int concurrencyLevel = numProcs * 2;
            const int initialCapacity = 10000;

            Cache = new ConcurrentDictionary<string, double>(concurrencyLevel, initialCapacity);
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
            Cache.TryAdd(peptide, net);
        }

        [Obsolete("Use TryGetValue")]
        public static double RetrieveValue(string peptide)
        {
            double predictedNET;
            if (TryGetValue(peptide, out predictedNET))
                return predictedNET;
            
            return 0;

        }
    }

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

        private void ConvertElutionTimeToNET(IEnumerable<Evidence> evidences, Dictionary<int, double> scanToTime)
        {
            var maxTime = scanToTime.Max(scanTimePair => scanTimePair.Value);
            var minTime = scanToTime.Min(scanTimePair => scanTimePair.Value);

            List<int> lstScans = scanToTime.Keys.ToList();
            lstScans.Sort();

            foreach (var evidence in evidences)
            {              
                double observedTime;

                if (!scanToTime.TryGetValue(evidence.Scan, out observedTime))
                {
                    // Exact match not found; find the closest match
                    var index = lstScans.BinarySearch(evidence.Scan);

                    if (index < 0)
                    {
                        int indexSmaller = ~index - 1;

                        if (indexSmaller < 0)
                            indexSmaller = 0;

                        double timeStart;
                        double timeEnd;

                        scanToTime.TryGetValue(lstScans[indexSmaller], out timeStart);

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
            int minimumColCount = Math.Max(scanColIndex, timeColIndex) + 1;

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
        public void CalculatePredictedNet(IRetentionTimePredictor predictor, IEnumerable<Evidence> evidences)
        {

            Parallel.ForEach(evidences, evidence =>
            {
                evidence.PredictedNet = ComputePeptideNET(evidence.CleanPeptide, predictor);
            });

        }

        public double ComputePeptideNET(string peptide, IRetentionTimePredictor predictor)
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


}