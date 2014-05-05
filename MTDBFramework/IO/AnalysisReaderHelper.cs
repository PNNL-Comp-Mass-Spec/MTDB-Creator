#region Namespaces

using System.Collections.Generic;
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
            double maxScan = evidences.Max(result => result.Scan);
            double minScan = evidences.Min(result => result.Scan);

            foreach (var evidence in evidences)
            {
                evidence.ObservedNet = (evidence.Scan - minScan) / (maxScan - minScan);
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