﻿#region Namespaces

using System.Collections.Generic;

#endregion

namespace Regressor.Algorithms
{
    public class LinearRegressionResult
    {
        public LinearRegressionResult()
        {
            Slope = 1;
            Intercept = 0;
            RSquared = 1;
            Coefficients = new [] { 0.0, 1.0 };
        }

        public double RSquared { get; set; }
        public double Intercept { get; set; }
        public double Slope { get; set; }
        public IEnumerable<double> Coefficients { get; set; }
    }
}
