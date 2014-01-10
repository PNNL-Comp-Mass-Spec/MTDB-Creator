#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Data;
using Regressor.Algorithms;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    public class SpectralTargetAligner : ITargetAligner
    {
        public LinearRegressionResult AlignTargets(List<Target> targets, List<Target> baseline)
        {
            baseline.ForEach(x => x.PredictedNet = x.ObservedNet);

            return new LinearRegressionResult();
        }
    }
}
