#region Namespaces

using System.Collections.Generic;

#endregion

namespace Regressor.Algorithms
{
    public interface IRegressorAlgorithm<T>
    {
        T CalculateRegression(IEnumerable<double> observed, IEnumerable<double> predicted);
        double Transform(T regressionFunction, double observed);
        void SetPoints(double[] x, double[] y);
    }
}
