#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Data;
using PNNLOmics.Algorithms.Regression;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    public class SpectralTargetAligner : ITargetAligner
    {
        public LinearRegressionResult AlignTargets(List<Evidence> evidences, List<Evidence> baseline)
        {
            baseline.ForEach(x => x.PredictedNet = x.ObservedNet);

            return new LinearRegressionResult();
        }
    }
}
