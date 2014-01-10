using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Regressor.Algorithms
{
    public static class LinearRegressorFactory
    {
        public static IRegressorAlgorithm<LinearRegressionResult> Create(RegressionType type)
        {
            switch (type)
            {
                case RegressionType.LinearEm:
                    return new LinearModelEm();
                case RegressionType.MixtureRegression:
                    return new MixtureModelEM();
                default:
                    return null;
            }
        }
    }
}
