#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Data;
using FeatureAlignment.Algorithms.Regression;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    /// <summary>
    /// Target alignment for spectral data
    /// </summary>
    public class SpectralTargetAligner : ITargetAligner
    {
        /// <summary>
        /// Perform target alignment
        /// </summary>
        /// <param name="evidences">Evidences to align</param>
        /// <param name="baseline">Baseline evidences</param>
        /// <returns></returns>
        public LinearRegressionResult AlignTargets(List<Evidence> evidences, List<Evidence> baseline)
        {
            baseline.ForEach(x => x.PredictedNet = x.ObservedNet);

            return new LinearRegressionResult();
        }
    }
}
