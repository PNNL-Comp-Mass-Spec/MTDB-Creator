﻿#region Namespaces

using System.Collections.Generic;
using FeatureAlignment.Algorithms.Regression;
using MTDBFrameworkBase.Data;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    /// <summary>
    /// Target alignment for proteins
    /// </summary>
    public class ProteinTargetAligner : ITargetAligner
    {
        /// <summary>
        /// Perform target alignment
        /// </summary>
        /// <param name="evidences">Evidences to align</param>
        /// <param name="baseline">Baseline evidences</param>
        /// <returns></returns>
        public LinearRegressionResult AlignTargets(List<Evidence> evidences, List<Evidence> baseline)
        {
            evidences.ForEach(x => x.PredictedNet = x.ObservedNet);

            return new LinearRegressionResult();
        }
    }
}
