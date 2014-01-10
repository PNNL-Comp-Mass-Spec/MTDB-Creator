#region Namespaces

using System.Collections.Generic;
using System.Linq;
using MTDBFramework.Algorithms.RetentionTimePrediction;
using MTDBFramework.Data;

#endregion

namespace MTDBFramework.IO
{
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
            foreach (Target target in targets)
            {
                target.PredictedNet = predictor.GetElutionTime(target.CleanPeptide);
            }
        }
    }
}