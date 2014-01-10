#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Data;
using Regressor.Algorithms;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    public class ProteinTargetAligner : ITargetAligner
    {
        public LinearRegressionResult AlignTargets(List<Target> targets, List<Target> baseline)
        {
            targets.ForEach(x => x.PredictedNet = x.ObservedNet);

            return new LinearRegressionResult();
        }
    }
}
