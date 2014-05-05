#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

#endregion

namespace Regressor.Algorithms
{
    // TODO: Double check all calculations.  Doesn't calculate correctly.
    public class MixtureModelEm : IRegressorAlgorithm<LinearRegressionResult>
    {
        private double m_normalizingSlopeX;
        private double m_normalizingSlopeY;
        private double m_normalizingInterceptX;
        private double m_normalizingInterceptY;

        private readonly double m_minPercentageChangeToContinue;
        private readonly bool m_normalizeInput;

        private DenseMatrix m_x;
        private DenseMatrix m_yRegressed;
        private DenseMatrix m_y;
        private DenseMatrix m_likelihoods;
        private DenseMatrix m_coefficients;

        private readonly List<Double> m_stdevReal;
        private readonly List<Double> m_stdevNoise;
        private readonly List<Double> m_meanNoise;
        private readonly List<Double> m_probReal;
        private readonly List<Double> m_likelihoodsList;

        private int m_percentComplete;

        private readonly short m_order;
        private const int MaxIterationsToPerform = 250;

        private void CalculateCoefficient(short coeffNum, out double coeff)
        {
            int numPoints = m_x.RowCount;
            double numerator = 0;
            double denominator = 0;

            for (int index = 0; index < numPoints; index++)
            {
                double diff = m_y.At(index, 0) - m_yRegressed.At(index, 0);
                double missingX = m_x.At(index, coeffNum);
                double missingTerm = missingX * m_coefficients.At(coeffNum, 0);
                diff += missingTerm;

                double likelihoodReal = m_likelihoods.At(index, 0);
                double likelihoodNoise = m_likelihoods.At(index, 1);
                double probability = likelihoodReal / (likelihoodNoise + likelihoodReal);

                numerator += diff * missingX * probability;
                denominator += probability * missingX * missingX;
            }
            coeff = numerator / denominator;
        }

        private void CalculateNextPredictions()
        {
            m_yRegressed = m_x.Multiply(m_coefficients) as DenseMatrix;
        }

        private void SetupMatrices()
        {
            int numPoints = RegressionPoints.Count;
            m_x = new DenseMatrix(numPoints, m_order + 1);
            m_y = new DenseMatrix(numPoints, 1);
            m_yRegressed = new DenseMatrix(numPoints, 1);
            m_likelihoods = new DenseMatrix(numPoints, 2);
            m_coefficients = new DenseMatrix(m_order + 1, 1);

            // set up X, Y and weights vector. 
            for (int index = 0; index < numPoints; index++)
            {
                RegressionPts currentPoint = RegressionPoints[index];
                double val = 1;
                for (int coeffNum = 0; coeffNum < m_order + 1; coeffNum++)
                {
                    m_x.At(index, coeffNum, val);
                    val *= currentPoint.MdblX;
                }
                m_y.At(index, 0, currentPoint.MdblY);
            }
        }

        private double CalculateLikelihoodsMatrix(int iterationNum)
        {
            int numPoints = m_x.RowCount;
            double noiseSigma = m_stdevNoise[iterationNum];
            double noiseMean = m_meanNoise[iterationNum];
            double realSigma = m_stdevReal[iterationNum];
            double probabilityReal = m_probReal[iterationNum];

            double logLikelihood = 0;
            for (int index = 0; index < numPoints; index++)
            {
                double diff = m_x.At(index, 0) - m_yRegressed.At(index, 0);
                double standardizedNoise = (diff - noiseMean) / noiseSigma;
                double standardizedReal = diff / realSigma;

                double likelihoodReal = probabilityReal * Math.Exp(-1 * standardizedReal * standardizedReal) / (Math.Sqrt(2*Math.PI) * realSigma);
                double likelihoodNoise = (1 - probabilityReal) * Math.Exp(-1 * standardizedNoise * standardizedNoise) / ((Math.Sqrt(2*Math.PI)) * noiseSigma);

                m_likelihoods.At(index, 0, likelihoodReal);
                m_likelihoods.At(index, 1, likelihoodNoise);
                logLikelihood += Math.Log(likelihoodReal + likelihoodNoise);
            }
            m_likelihoodsList.Add(logLikelihood);
            return logLikelihood;
        }

        private void CalculateRealStdev(out double stdev)
        {
            int numPoints = m_x.RowCount;

            double sumSigmaSqW = 0;
            double sumW = 0;

            for (int index = 0; index < numPoints; index++)
            {
                double diff = m_y.At(index, 0) - m_yRegressed.At(index, 0);
                double likelihoodReal = m_likelihoods.At(index, 0);
                double likelihoodNoise = m_likelihoods.At(index, 1);
                double probability = likelihoodReal / (likelihoodNoise + likelihoodReal);
                sumSigmaSqW += probability * diff * diff;
                sumW += probability;
            }
            stdev = Math.Sqrt(sumSigmaSqW / sumW);
        }

        private void CalculateNoiseStdevAndMean(int iterationNum, out double stdev, out double mean)
        {
            int numPoints = m_x.RowCount;
            double sumSigmaSqW = 0;
            double sumW = 0;
            double sumWNoise = 0;
            double noiseMean = m_meanNoise[iterationNum];

            for (int index = 0; index < numPoints; index++)
            {
                double y = m_y.At(index, 0);
                double likelihoodReal = m_likelihoods.At(index, 0);
                double likelihoodNoise = m_likelihoods.At(index, 1);
                double probability = likelihoodNoise / (likelihoodNoise + likelihoodReal);

                sumSigmaSqW += probability * (y - noiseMean) * (y - noiseMean);
                sumWNoise += y * probability;
                sumW += probability;
            }
            mean = sumWNoise / sumW;
            stdev = Math.Sqrt(sumSigmaSqW / sumW);
        }

        private void CalculateRealProbability(out double probability)
        {
            int numPoints = m_x.RowCount;
            double normalProbSum = 0;

            for (int index = 0; index < numPoints; index++)
            {
                double likelihoodReal = m_likelihoods.At(index, 0);
                double likelihoodNoise = m_likelihoods.At(index, 1);
                normalProbSum += likelihoodReal / (likelihoodNoise + likelihoodReal);
            }
            normalProbSum /= numPoints;
            probability = normalProbSum;
        }

        private void NormalizeXAndYs()
        {
            int numPoints = RegressionPoints.Count;
            if (numPoints == 0)
                throw new InvalidOperationException("Zero elements passed to NormalizeXAndYs");

            var vectXs = new List<double>();
            var vectYs = new List<double>();
            vectXs.Capacity = numPoints;
            vectYs.Capacity = numPoints;

            for (int index = 0; index < numPoints; index++)
            {
                RegressionPts currentPoint = RegressionPoints[index];
                vectXs.Add(currentPoint.MdblX);
                vectYs.Add(currentPoint.MdblY);
            }

            vectXs.Sort();
            vectYs.Sort();

            int medianIndex;
            if (numPoints % 2 == 0)
            {
                medianIndex = (numPoints - 1) / 2;
            }
            else
            {
                medianIndex = numPoints / 2;
            }

            double xMedian = vectXs[medianIndex];
            double yMedian = vectYs[medianIndex];
            double xMin = vectXs[0];
            double xMax = vectXs[numPoints - 1];
            double yMin = vectYs[0];
            double yMax = vectYs[numPoints - 1];

            m_normalizingInterceptX = -xMedian / (xMax - xMin);
            m_normalizingSlopeX = 1 / (xMax - xMin);

            m_normalizingInterceptY = -yMedian / (yMax - yMin);
            m_normalizingSlopeY = 1 / (yMax - yMin);

            for (int index = 0; index < numPoints; index++)
            {
                RegressionPoints[index].MdblX = RegressionPoints[index].MdblX * m_normalizingSlopeX + m_normalizingInterceptX;
                RegressionPoints[index].MdblY = RegressionPoints[index].MdblY * m_normalizingSlopeY + m_normalizingInterceptY;
            }
        }

        public List<RegressionPts> RegressionPoints;
        public List<RegressionPts> DistinctRegressionPoints;
        public LinearRegressionResult RegressionResult;
        

        /// <summary>
        /// 
        /// </summary>
        public MixtureModelEm()
        {
            RegressionPoints = new List<RegressionPts>();
            DistinctRegressionPoints = new List<RegressionPts>();
            RegressionResult = new LinearRegressionResult();

            m_stdevReal = new List<double>();
            m_stdevNoise = new List<double>();
            m_meanNoise = new List<double>();
            m_probReal = new List<double>();
            m_likelihoodsList = new List<double>();

            m_x = null;
            m_y = null;
            m_coefficients = null;
            m_likelihoods = null;
            m_yRegressed = null;

            m_percentComplete = 0;
            m_order = 1;
            m_normalizeInput = true;
            m_minPercentageChangeToContinue = 0.0001;
        }

        public LinearRegressionResult CalculateRegression(IEnumerable<double> observed, IEnumerable<double> predicted)
        {
            SetPoints(observed as double[], predicted as double[]);

            RegressionPoints.Sort();
            DistinctRegressionPoints = new List<RegressionPts>(RegressionPoints.Distinct());

            if (m_normalizeInput)
                NormalizeXAndYs();

            SetupMatrices();
            int numPts = RegressionPoints.Count;

            // CALCULATE FIRST SET OF COEFFICIENTS HERE .. 
            // Remember that the coefficients should be: 
            // (XX')-1X             
            var xTranspose = m_x.Transpose() as DenseMatrix;
            if (xTranspose != null)
            {
                var xprimeX = xTranspose.Multiply(m_x) as DenseMatrix;

                if (xprimeX == null)
                {
                    throw new InvalidCastException("Unable to multiply mobj_X with XTranspose in clsMixtureModelRegressionEM::CalculateRegressionFunction");
                }

                var invXprimeX = xprimeX.Inverse() as DenseMatrix;

                if (invXprimeX == null)
                {
                    throw new InvalidCastException("Unable to invert xprime_x in clsMixtureModelRegressionEM::CalculateRegressionFunction");
                }

                var invXprimeXxTrans = invXprimeX.Multiply(xTranspose) as DenseMatrix;

                if (invXprimeXxTrans == null)
                {
                    throw new InvalidCastException("to multiple inv_xprime_wx and mobj_X in clsMixtureModelRegressionEM::CalculateRegressionFunction");
                }

                m_coefficients = invXprimeXxTrans.Multiply(m_y) as DenseMatrix;
            }

            if (m_coefficients == null)
            {
                throw new InvalidOperationException("Unable to multiple inv_xprime_x and mobj_X in clsMixtureModelRegressionEM::CalculateRegressionFunction");
            }

            var nextCoefficients = new DenseMatrix(m_order + 1, 1);
            var bestCoefficients = new DenseMatrix(m_order + 1, 1);

            m_likelihoodsList.Clear();
            m_meanNoise.Clear();
            m_probReal.Clear();
            m_stdevNoise.Clear();
            m_stdevReal.Clear();

            double sumY = 0;
            double sumYy = 0;
            for (int ptNum = 0; ptNum < numPts; ptNum++)
            {
                double y = RegressionPoints[ptNum].MdblY;
                sumY += y;
                sumYy += y * y;
            }
            double noiseMean = sumY / numPts;
            double noiseStdev = Math.Sqrt((sumYy - sumY * sumY / numPts) / numPts);

            m_meanNoise.Add(noiseMean);
            m_stdevNoise.Add(noiseStdev);
            m_stdevReal.Add(noiseStdev / 10);
            m_probReal.Add(0.5);

            double maxLikelihood = -1 * double.MaxValue;
            for (int iterationNum = 0; iterationNum < MaxIterationsToPerform; iterationNum++)
            {
                m_percentComplete = 20 + (80 * iterationNum) / MaxIterationsToPerform;
                CalculateNextPredictions();
                double likelihood = CalculateLikelihoodsMatrix(iterationNum);
                if (likelihood > maxLikelihood)
                {
                    maxLikelihood = likelihood;
                    // set the best set of coefficients. 
                    for (short coefficientNum = 0; coefficientNum < m_order + 1; coefficientNum++)
                        bestCoefficients.At(coefficientNum, 0, m_coefficients.At(coefficientNum, 0));
                }

                double probabilityReal;
                CalculateRealProbability(out probabilityReal);
                m_probReal.Add(probabilityReal);

                double stdevReal;
                CalculateRealStdev(out stdevReal);
                m_stdevReal.Add(stdevReal);

                double meanNoise, stdevNoise;
                CalculateNoiseStdevAndMean(iterationNum, out stdevNoise, out meanNoise);
                m_stdevNoise.Add(stdevNoise);
                m_meanNoise.Add(meanNoise);

                for (short coefficientNum = 0; coefficientNum < m_order + 1; coefficientNum++)
                {
                    double coefficient;
                    CalculateCoefficient(coefficientNum, out coefficient);
                    nextCoefficients.At(coefficientNum, 0, coefficient);
                }

                // set the next set of coefficient. 
                for (short coefficientNum = 0; coefficientNum < m_order + 1; coefficientNum++)
                    m_coefficients.At(coefficientNum, 0, nextCoefficients.At(coefficientNum, 0));
                if (iterationNum > 0)
                {
                    double likelihoodChange = Math.Abs(m_likelihoodsList[iterationNum] - m_likelihoodsList[iterationNum - 1]);
                    double minLikelihoodChange = Math.Abs(m_minPercentageChangeToContinue * m_likelihoodsList[iterationNum - 1]);
                    if (likelihoodChange < minLikelihoodChange)
                        break;
                }
            }

            for(short coefficientNum = 0; coefficientNum < m_order + 1; coefficientNum++)
                m_coefficients.At(coefficientNum, 0, bestCoefficients.At(coefficientNum, 0));

            // Calculate Slope
            RegressionResult.Slope = m_coefficients.At(1, 0);
            if (m_normalizeInput)
            {
                RegressionResult.Slope = RegressionResult.Slope * m_normalizingSlopeX / m_normalizingSlopeY;
            }
            
            // Calculate Intercept
            RegressionResult.Intercept = m_coefficients.At(0, 0);
            if(m_normalizeInput)
            {
                double slope = m_coefficients.At(1, 0);
                RegressionResult.Intercept = (RegressionResult.Intercept - m_normalizingInterceptY + slope * m_normalizingInterceptX) / m_normalizingSlopeY; 
            }
            return RegressionResult;
        }

        public double Transform(LinearRegressionResult regressionFunction, double observed)
        {
            throw new NotImplementedException();
        }


        public void SetPoints(double[] x, double[] y)
        {
            RegressionPoints.Clear();
            double minScan = 1024 * 1024 * 16;
            double maxScan = -1 * minScan;

            foreach (var val in x)
            {
                if (val < minScan)
                    minScan = val;
                if (val > maxScan)
                    maxScan = val;
            }

            const double percentToIgnore = 0.4;
            var lowerX = minScan + (maxScan - minScan) * (percentToIgnore / 2.0);
            var upperX = maxScan - (maxScan - minScan) * (percentToIgnore / 2.0);

            for (int ptNum = 0; ptNum < x.Length; ptNum++)
            {
                var pt = new RegressionPts { MdblX = x[ptNum], MdblY = y[ptNum] };

                if (pt.MdblX > lowerX && pt.MdblX < upperX)
                    RegressionPoints.Add(pt);
            }
        }
    }
}