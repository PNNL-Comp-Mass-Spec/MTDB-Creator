 #region Namespaces

using System.Collections.Generic;
using MTDBFramework.Data;
using FeatureAlignment.Algorithms.Regression;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    /// <summary>
    /// Interface for target alignment
    /// </summary>
    public interface ITargetAligner
    {
        /// <summary>
        /// Perform target alignment
        /// </summary>
        /// <param name="evidences">Evidences to align</param>
        /// <param name="baseline">Baseline evidences</param>
        /// <returns></returns>
        LinearRegressionResult AlignTargets(List<Evidence> evidences, List<Evidence> baseline);
    }
}
