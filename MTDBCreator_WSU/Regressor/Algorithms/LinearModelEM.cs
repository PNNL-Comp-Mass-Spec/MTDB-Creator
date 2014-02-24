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
        private DenseMatrix _mobjX;
        private DenseMatrix _mobjXTranspose;
        private DenseMatrix _mobjY;
        private DenseMatrix _mobjWeights;

        private double _mdblUnifU;
        private double _mdblStdev;
        private const int NumIterationsToPerform = 20;

        private void CalculateProbabilitiesAndWeightMatrix()
        {
            int numPoints = _mobjX.RowCount;
            double normalProbSum = 0;
            double sumSquareDiff = 0;
            double sumSquareDiffForRsq = 0;
            double sumSquareY = 0;
            double sumY = 0;

            for (int index = 0; index < numPoints; index++)
            {
                double diff = _mobjY.At(index, 0) - (RegressionResult.Slope * _mobjX.At(index, 0) + RegressionResult.Intercept);
                double exponent = Math.Exp(-0.5 * (diff / _mdblStdev) * (diff / _mdblStdev));
                double normalizer = _mdblStdev * Math.Sqrt(2 * Math.PI);
                double probNormalConditional = exponent / normalizer;
                double probNormalPosterior = (PercentNormal * probNormalConditional) / (PercentNormal * probNormalConditional + (1 - PercentNormal) * _mdblUnifU);
                _mobjWeights.At(index, index, probNormalPosterior);
                normalProbSum += probNormalPosterior;
                sumSquareDiff += probNormalPosterior * diff * diff;
                sumSquareDiffForRsq += diff * diff;
                sumSquareY += (_mobjY.At(index, 0) * _mobjY.At(index, 0));
                sumY += _mobjY.At(index, 0);
            }

            PercentNormal = normalProbSum / numPoints;
            _mdblStdev = Math.Sqrt(sumSquareDiff / (normalProbSum));
            double yVar = sumSquareY - sumY * sumY / numPoints;
            if (yVar == 0)
            {
                RegressionResult.RSquared = 0;
                throw new Exception("All y values are the same");
            }
            else
            {
                RegressionResult.RSquared = 1 - sumSquareDiffForRsq / yVar;
            }
        }

        private void CalculateSlopeInterceptEstimates()
        {
            var wx = _mobjWeights.Multiply(_mobjX) as DenseMatrix;

            if (wx == null)
            {
                throw new InvalidOperationException();
            }
            var xprimeWx = _mobjXTranspose.Multiply(wx) as DenseMatrix; 
            if (xprimeWx == null)
            {
                throw new InvalidOperationException();
            }
            var invXprimeWx = xprimeWx.Inverse() as DenseMatrix;
            if (invXprimeWx == null)
            {
                throw new InvalidOperationException();
            }
            DenseMatrix wy;
            wy = _mobjWeights.Multiply(_mobjY) as DenseMatrix;
            if (wy == null)
            {
                throw new InvalidOperationException();
            }
            var xprimeWy = _mobjXTranspose.Multiply(wy) as DenseMatrix;
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
            var numPoints = _mobjX.RowCount;
            var maxDiff = -1 * double.MaxValue;
            var minDiff = double.MaxValue;
            var maxY = -1 * double.MaxValue;

            for (int index = 0; index < numPoints; index++)
            {
                double diff = _mobjY.At(index, 0) - (RegressionResult.Slope * _mobjX.At(index, 0) + RegressionResult.Intercept);
                if (diff > maxDiff)
                    maxDiff = diff;
                if (diff < minDiff)
                    minDiff = diff;
                if (_mobjY.At(index, 0) > maxY)
                    maxY = _mobjY.At(index, 0);
            }
            //_mdblUnifU = 1.0 / (maxDiff - minDiff);
            _mdblUnifU = 1.0 / maxY;
        }

        private double CalculateInitialStdev()
        {
            int numPoints = _mobjX.RowCount;

            _mdblStdev = 0;
            for (int index = 0; index < numPoints; index++)
            {
                double diff = _mobjY.At(index, 0) - (RegressionResult.Slope * _mobjX.At(index, 0) + RegressionResult.Intercept);
                _mdblStdev += diff * diff;
            }
            _mdblStdev /= numPoints;
            _mdblStdev = Math.Sqrt(_mdblStdev);
            return _mdblStdev;
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

            _mobjX = new DenseMatrix(numPoints, 2);
            _mobjY = new DenseMatrix(numPoints, 1);
            _mobjWeights = new DenseMatrix(numPoints, numPoints);
            PercentNormal = 0.5;

            // set up X, Y and weights vector. 
            double sumX = 0;
            double sumXX = 0;

            for (int index = 0; index < numPoints; index++)
            {
                RegressionPts currentPoint = RegressionPoints[index];

                _mobjX.At(index, 0, currentPoint.MdblX);
                sumX += currentPoint.MdblX;
                sumXX += (currentPoint.MdblX * currentPoint.MdblX);
                _mobjX.At(index, 1, 1);

                _mobjY.At(index, 0, currentPoint.MdblY);
                for (int colNum = 0; colNum < numPoints; colNum++)
                {
                    _mobjWeights.At(index, colNum, 0);
                }
                _mobjWeights.At(index, index, 1);
            }

            if (sumX * sumX == numPoints * sumXX)
            {
                RegressionResult.Intercept = 0;
                RegressionResult.Slope = 0;
                RegressionResult.RSquared = 0;
                throw new Exception("All X values cannot be same for linear regression");
            }

            _mobjXTranspose = _mobjX.Transpose() as DenseMatrix;
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
