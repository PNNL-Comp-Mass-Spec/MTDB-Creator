 #region Namespaces

using System.Collections.Generic;
using MTDBFramework.Data;
using PNNLOmics.Algorithms.Regression;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    public interface ITargetAligner
    {
        LinearRegressionResult AlignTargets(List<Evidence> evidences, List<Evidence> baseline);
    }
}
