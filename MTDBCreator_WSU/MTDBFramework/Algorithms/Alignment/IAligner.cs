#region Namespaces

using System.Collections.Generic;
using MTDBFramework.Data;
using Regressor.Algorithms;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    public interface ITargetAligner
    {
        LinearRegressionResult AlignTargets(List<Target> targets, List<Target> baseline);
    }
}
