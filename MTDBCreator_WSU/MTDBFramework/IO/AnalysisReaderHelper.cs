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
        private static Dictionary<string, double> m_cache;

        static PeptideCache()
        {
            m_cache = new Dictionary<string, double>();
        }

        public static void Clear()
        {
            m_cache.Clear();
        }

        public static bool HasValue(string peptide)
        {
            return m_cache.ContainsKey(peptide);
        }

        public static void Add(string peptide, double net)
        {
            m_cache.Add(peptide, net);
        }

        public static double RetrieveValue(string peptide)
        {
            return m_cache[peptide];
        }
    }

    public class CacheAccessor
    {
        public double PredictPeptide(string peptide, IRetentionTimePredictor predictor)
        {
            double net = 0;
            bool hasBeenPredicted = PeptideCache.HasValue(peptide);
            if (!hasBeenPredicted)
            {
                net = predictor.GetElutionTime(peptide);
                PeptideCache.Add(peptide, net);
                return net;
            }
            return PeptideCache.RetrieveValue(peptide);
        }
    }

    public static class AnalysisReaderHelper
    {
        public static void CalculateObservedNet(IEnumerable<Target> targets)
        {
            double maxScan = targets.Max(result => result.Scan);
            double minScan = targets.Min(result => result.Scan);

            foreach (Target target in targets)
            {
                target.ObservedNet = (target.Scan - minScan) / (maxScan - minScan);
            }
        }

        public static void CalculatePredictedNet(IRetentionTimePredictor predictor, IEnumerable<Target> targets)
        {
            CacheAccessor pepCache = new CacheAccessor();
            foreach (Target target in targets)
            {
                target.PredictedNet = pepCache.PredictPeptide(target.CleanPeptide, predictor);
                // Old version of predicting the NET not utilizing the peptide cache
                // target.PredictedNet = predictor.GetElutionTime(target.CleanPeptide);
            }
        }
    }
}