#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

#endregion

namespace Regressor.Algorithms
{
    public class LinearModelEm : IRegressorAlgorithm<LinearRegressionResult>
    {
        private DenseMatrix m_x;
        private DenseMatrix m_xTranspose;
        private DenseMatrix m_y;
        private DenseMatrix m_weights;

        private double m_unifU;
        private double m_stdev;
        private const int NumIterationsToPerform = 20;

        private void CalculateProbabilitiesAndWeightMatrix()
        {
            int numPoints = m_x.RowCount;
            double normalProbSum = 0;
            double sumSquareDiff = 0;
            double sumSquareDiffForRsq = 0;
            double sumSquareY = 0;
            double sumY = 0;

            for (int index = 0; index < numPoints; index++)
            {
                double diff = m_y.At(index, 0) - (RegressionResult.Slope * m_x.At(index, 0) + RegressionResult.Intercept);
                double exponent = Math.Exp(-0.5 * (diff / m_stdev) * (diff / m_stdev));
                double normalizer = m_stdev * Math.Sqrt(2 * Math.PI);
                double probNormalConditional = exponent / normalizer;
                double probNormalPosterior = (PercentNormal * probNormalConditional) / (PercentNormal * probNormalConditional + (1 - PercentNormal) * m_unifU);
                m_weights.At(index, index, probNormalPosterior);
                normalProbSum += probNormalPosterior;
                sumSquareDiff += probNormalPosterior * diff * diff;
                sumSquareDiffForRsq += diff * diff;
                sumSquareY += (m_y.At(index, 0) * m_y.At(index, 0));
                sumY += m_y.At(index, 0);
            }

            PercentNormal = normalProbSum / numPoints;
            m_stdev = Math.Sqrt(sumSquareDiff / (normalProbSum));
            double yVar = sumSquareY - sumY * sumY / numPoints;
            if (Math.Abs(yVar) < double.Epsilon)
            {
                RegressionResult.RSquared = 0;
                throw new Exception("All y values are the same");
            }
            RegressionResult.RSquared = 1 - sumSquareDiffForRsq / yVar;
        }

        private void CalculateSlopeInterceptEstimates()
        {
            var wx = m_weights.Multiply(m_x) as DenseMatrix;

            if (wx == null)
            {
                throw new InvalidOperationException();
            }
            var xprimeWx = m_xTranspose.Multiply(wx) as DenseMatrix; 
            if (xprimeWx == null)
            {
                throw new InvalidOperationException();
            }
            var invXprimeWx = xprimeWx.Inverse() as DenseMatrix;
            if (invXprimeWx == null)
            {
                throw new InvalidOperationException();
            }
            var wy = m_weights.Multiply(m_y) as DenseMatrix;
            if (wy == null)
            {
                throw new InvalidOperationException();
            }
            var xprimeWy = m_xTranspose.Multiply(wy) as DenseMatrix;
            if (xprimeWy == null)
            {
                throw new InvalidOperationException();
            }
            var beta = invXprimeWx.Multiply(xprimeWy) as DenseMatrix;
            if (beta == null)
            {
                throw new InvalidOperationException();
            }

            
            RegressionResult.Slope = beta.At(0, 0); // Create slope
            RegressionResult.Intercept = beta.At(1, 0); // Create intercept
            var numPoints = m_x.RowCount;
            var maxDiff = -1 * double.MaxValue;
            var minDiff = double.MaxValue;
            var maxY = -1 * double.MaxValue;

            for (int index = 0; index < numPoints; index++)
            {
                double diff = m_y.At(index, 0) - (RegressionResult.Slope * m_x.At(index, 0) + RegressionResult.Intercept);
                if (diff > maxDiff)
                    maxDiff = diff;
                if (diff < minDiff)
                    minDiff = diff;
                if (m_y.At(index, 0) > maxY)
                    maxY = m_y.At(index, 0);
            }
            //_mdblUnifU = 1.0 / (maxDiff - minDiff);
            m_unifU = 1.0 / maxY;
        }

        private void CalculateInitialStdev()
        {
            int numPoints = m_x.RowCount;

            m_stdev = 0;
            for (int index = 0; index < numPoints; index++)
            {
                double diff = m_y.At(index, 0) - (RegressionResult.Slope * m_x.At(index, 0) + RegressionResult.Intercept);
                m_stdev += diff * diff;
            }
            m_stdev /= numPoints;
            m_stdev = Math.Sqrt(m_stdev);
        }

        public List<RegressionPts> RegressionPoints { get; set; }
        public LinearRegressionResult RegressionResult { get; set; }

        public double PercentNormal { get; set; }

        public LinearModelEm()
        {
            RegressionPoints = new List<RegressionPts>();
            RegressionResult = new LinearRegressionResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPoints(double[] x, double[] y)
        {
            RegressionPoints.Clear();
            double minScan = 1024 * 1024 * 16;
            double maxScan = -1 * minScan;

            foreach (double val in x)
            {
                if (val < minScan)
                    minScan = val;
                if (val > maxScan)
                    maxScan = val;
            }

            const double percentToIgnore = 0.0;

            /*Old regression model*/
            // var lowerX = minScan;
            // var upperX = maxScan - (maxScan - minScan) * percentToIgnore;

            /*New regression model*/
            var lowerX = minScan + (minScan + maxScan) * (percentToIgnore / 2.0);
            var upperX = maxScan - (maxScan - minScan) * (percentToIgnore / 2.0);

            for (int ptNum = 0; ptNum < x.Length; ptNum++)
            {
//                if (Math.Abs(x[ptNum] - y[ptNum]) < 0.2)
                {
                    var pt = new RegressionPts { MdblX = x[ptNum], MdblY = y[ptNum] };
                    if (pt.MdblX > lowerX && pt.MdblX < upperX)
                        RegressionPoints.Add(pt);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observed"></param>
        /// <param name="predicted"></param>
        /// <returns></returns>
        public LinearRegressionResult CalculateRegression(IEnumerable<double> observed, IEnumerable<double> predicted)
        {
            
            SetPoints(observed.ToArray(), predicted.ToArray());
            int numPoints = RegressionPoints.Count;

            m_x = new DenseMatrix(numPoints, 2);
            m_y = new DenseMatrix(numPoints, 1);
            m_weights = new DenseMatrix(numPoints, numPoints);
            PercentNormal = 0.5;

            // set up X, Y and weights vector. 
            double sumX = 0;
            double sumXx = 0;

            for (int index = 0; index < numPoints; index++)
            {
                RegressionPts currentPoint = RegressionPoints[index];

                m_x.At(index, 0, currentPoint.MdblX);
                sumX += currentPoint.MdblX;
                sumXx += (currentPoint.MdblX * currentPoint.MdblX);
                m_x.At(index, 1, 1);

                m_y.At(index, 0, currentPoint.MdblY);
                for (int colNum = 0; colNum < numPoints; colNum++)
                {
                    m_weights.At(index, colNum, 0);
                }
                m_weights.At(index, index, 1);
            }

            if (Math.Abs(sumX * sumX - numPoints * sumXx) < double.Epsilon)
            {
                RegressionResult.Intercept = 0;
                RegressionResult.Slope = 0;
                RegressionResult.RSquared = 0;
                throw new Exception("All X values cannot be same for linear regression");
            }

            m_xTranspose = m_x.Transpose() as DenseMatrix;
            CalculateSlopeInterceptEstimates();
            CalculateInitialStdev();
            CalculateProbabilitiesAndWeightMatrix();

            for (int iterationNum = 0; iterationNum < NumIterationsToPerform; iterationNum++)
            {
                CalculateSlopeInterceptEstimates();
                CalculateProbabilitiesAndWeightMatrix();
                if (PercentNormal < 0.01)
                {
                    throw new Exception("PercentNormal < 0.01");
                }
            }

            return RegressionResult;
        }

        /// <summary>
        /// Transforms the observed value based on the regression function's slope and intercept.
        /// </summary>
        /// <param name="regressionFunction"></param>
        /// <param name="observed"></param>
        /// <returns></returns>
        public double Transform(LinearRegressionResult regressionFunction, double observed)
        {
            return (regressionFunction.Slope * observed + regressionFunction.Intercept);
        }
    }
}
