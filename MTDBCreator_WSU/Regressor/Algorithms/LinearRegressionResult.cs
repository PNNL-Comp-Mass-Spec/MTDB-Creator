#region Namespaces

using System.Collections.Generic;

#endregion

namespace Regressor.Algorithms
{
    public class LinearRegressionResult
    {
        public LinearRegressionResult()
        {
            this.Slope = 1;
            this.Intercept = 0;
            this.RSquared = 1;
            this.Coefficients = new double[] { 0.0, 1.0 };
        }

        public double RSquared { get; set; }
        public double Intercept { get; set; }
        public double Slope { get; set; }
        public IEnumerable<double> Coefficients { get; set; }
    }
}
