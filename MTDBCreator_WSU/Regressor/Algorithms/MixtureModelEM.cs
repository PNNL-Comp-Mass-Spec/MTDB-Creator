#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;

#endregion

namespace Regressor.Algorithms
{
    // TODO: Double check all calculations.  Doesn't calculate correctly.
    public class MixtureModelEM : IRegressorAlgorithm<LinearRegressionResult>
    {
        private double _normalizingSlopeX;
        private double _normalizingSlopeY;
        private double _normalizingInterceptX;
        private double _normalizingInterceptY;

        private double mdbl_min_percentage_change_to_continue;
        private bool mbln_normalize_input;

        private DenseMatrix mobj_X;
        private DenseMatrix mobj_YRegressed;
        private DenseMatrix mobj_Y;
        private DenseMatrix mobj_Likelihoods;
        private DenseMatrix mobj_Coefficients;

        private List<Double> mvect_stdevReal;
        private List<Double> mvect_stdevNoise;
        private List<Double> mvect_meanNoise;
        private List<Double> mvect_probReal;
        private List<Double> mvect_Likelihoods;

        private short mshort_order;
        private int mint_percent_complete;
        private static int MAX_ITERATIONS_TO_PERFORM = 250;

        private const double PI = 3.14159265358979323846;
        private const double SQRT2PI = 2.506628274631;


        private void CalculateCoefficient(short coeffNum, ref double coeff)
        {
            int numPoints = mobj_X.RowCount;
            double numerator = 0;
            double denominator = 0;

            for (int index = 0; index < numPoints; index++)
            {
                double diff = mobj_Y.At(index, 0) - mobj_YRegressed.At(index, 0);
                double missingX = mobj_X.At(index, coeffNum);
                double missing_term = missingX * mobj_Coefficients.At(coeffNum, 0);
                diff += missing_term;

                double likelihood_real = mobj_Likelihoods.At(index, 0);
                double likelihood_noise = mobj_Likelihoods.At(index, 1);
                double probability = likelihood_real / (likelihood_noise + likelihood_real);

                numerator += diff * missingX * probability;
                denominator += probability * missingX * missingX;
            }
            coeff = numerator / denominator;
        }

        private void CalculateNextPredictions()
        {
            mobj_YRegressed = mobj_X.Multiply(mobj_Coefficients) as DenseMatrix;
        }

        private void SetupMatrices()
        {
            int numPoints = RegressionPoints.Count;
            mobj_X = new DenseMatrix(numPoints, mshort_order + 1);
            mobj_Y = new DenseMatrix(numPoints, 1);
            mobj_YRegressed = new DenseMatrix(numPoints, 1);
            mobj_Likelihoods = new DenseMatrix(numPoints, 2);
            mobj_Coefficients = new DenseMatrix(mshort_order + 1, 1);

            // set up X, Y and weights vector. 
            for (int index = 0; index < numPoints; index++)
            {
                RegressionPts currentPoint = RegressionPoints[index];
                double val = 1;
                for (int coeffNum = 0; coeffNum < mshort_order + 1; coeffNum++)
                {
                    mobj_X.At(index, coeffNum, val);
                    val *= currentPoint.MdblX;
                }
                mobj_Y.At(index, 0, currentPoint.MdblY);
            }
        }

        private double CalculateLikelihoodsMatrix(int iterationNum)
        {
            int numPoints = mobj_X.RowCount;
            double noise_sigma = mvect_stdevNoise[iterationNum];
            double noise_mean = mvect_meanNoise[iterationNum];
            double real_sigma = mvect_stdevReal[iterationNum];
            double probability_real = mvect_probReal[iterationNum];

            double log_likelihood = 0;
            for (int index = 0; index < numPoints; index++)
            {
                double diff = mobj_X.At(index, 0) - mobj_YRegressed.At(index, 0);
                double standardizedNoise = (diff - noise_mean) / noise_sigma;
                double standardizedReal = diff / real_sigma;

                double likelihood_real = probability_real * Math.Exp(-1 * standardizedReal * standardizedReal) / (SQRT2PI * real_sigma);
                double likelihood_noise = (1 - probability_real) * Math.Exp(-1 * standardizedNoise * standardizedNoise) / (SQRT2PI * noise_sigma);

                mobj_Likelihoods.At(index, 0, likelihood_real);
                mobj_Likelihoods.At(index, 1, likelihood_noise);
                log_likelihood += Math.Log(likelihood_real + likelihood_noise);
            }
            mvect_Likelihoods.Add(log_likelihood);
            return log_likelihood;
        }

        private void CalculateRealStdev(int iterationNum, ref double stdev)
        {
            int numPoints = mobj_X.RowCount;

            double sum_sigma_sq_w = 0;
            double sum_w = 0;

            for (int index = 0; index < numPoints; index++)
            {
                double diff = mobj_Y.At(index, 0) - mobj_YRegressed.At(index, 0);
                double likelihood_real = mobj_Likelihoods.At(index, 0);
                double likelihood_noise = mobj_Likelihoods.At(index, 1);
                double probability = likelihood_real / (likelihood_noise + likelihood_real);
                sum_sigma_sq_w += probability * diff * diff;
                sum_w += probability;
            }
            stdev = Math.Sqrt(sum_sigma_sq_w / sum_w);
        }

        private void CalculateNoiseStdevAndMean(int iterationNum, ref double stdev, ref double mean)
        {
            int numPoints = mobj_X.RowCount;
            double sum_sigma_sq_w = 0;
            double sum_w = 0;
            double sum_w_noise = 0;
            double noise_mean = mvect_meanNoise[iterationNum];

            for (int index = 0; index < numPoints; index++)
            {
                double y = mobj_Y.At(index, 0);
                double likelihood_real = mobj_Likelihoods.At(index, 0);
                double likelihood_noise = mobj_Likelihoods.At(index, 1);
                double probability = likelihood_noise / (likelihood_noise + likelihood_real);

                sum_sigma_sq_w += probability * (y - noise_mean) * (y - noise_mean);
                sum_w_noise += y * probability;
                sum_w += probability;
            }
            mean = sum_w_noise / sum_w;
            stdev = Math.Sqrt(sum_sigma_sq_w / sum_w);
        }

        private void CalculateRealProbability(int iterationNum, ref double probability)
        {
            int numPoints = mobj_X.RowCount;
            double normal_prob_sum = 0;

            for (int index = 0; index < numPoints; index++)
            {
                double likelihood_real = mobj_Likelihoods.At(index, 0);
                double likelihood_noise = mobj_Likelihoods.At(index, 1);
                normal_prob_sum += likelihood_real / (likelihood_noise + likelihood_real);
            }
            normal_prob_sum /= numPoints;
            probability = normal_prob_sum;
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

            int medianIndex = 0;
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

            _normalizingInterceptX = -xMedian / (xMax - xMin);
            _normalizingSlopeX = 1 / (xMax - xMin);

            _normalizingInterceptY = -yMedian / (yMax - yMin);
            _normalizingSlopeY = 1 / (yMax - yMin);

            for (int index = 0; index < numPoints; index++)
            {
                RegressionPoints[index].MdblX = RegressionPoints[index].MdblX * _normalizingSlopeX + _normalizingInterceptX;
                RegressionPoints[index].MdblY = RegressionPoints[index].MdblY * _normalizingSlopeY + _normalizingInterceptY;
            }
        }

        public List<RegressionPts> RegressionPoints;
        public List<RegressionPts> DistinctRegressionPoints;
        public LinearRegressionResult RegressionResult;

        /// <summary>
        /// 
        /// </summary>
        public MixtureModelEM()
        {
            RegressionPoints = new List<RegressionPts>();
            DistinctRegressionPoints = new List<RegressionPts>();
            RegressionResult = new LinearRegressionResult();

            mvect_stdevReal = new List<double>();
            mvect_stdevNoise = new List<double>();
            mvect_meanNoise = new List<double>();
            mvect_probReal = new List<double>();
            mvect_Likelihoods = new List<double>();

            mobj_X = null;
            mobj_Y = null;
            mobj_Coefficients = null;
            mobj_Likelihoods = null;
            mobj_YRegressed = null;

            mint_percent_complete = 0;
            mshort_order = 1;
            mbln_normalize_input = true;
            mdbl_min_percentage_change_to_continue = 0.0001;
        }

        public LinearRegressionResult CalculateRegression(IEnumerable<double> observed, IEnumerable<double> predicted)
        {
            SetPoints(observed as double[], predicted as double[]);
            RegressionPoints.Sort();
            DistinctRegressionPoints = new List<RegressionPts>(RegressionPoints.Distinct());

            if (mbln_normalize_input)
                NormalizeXAndYs();

            SetupMatrices();
            int numPts = RegressionPoints.Count;

            // CALCULATE FIRST SET OF COEFFICIENTS HERE .. 
            // Remember that the coefficients should be: 
            // (XX')-1X             
            var xTranspose = mobj_X.Transpose() as DenseMatrix;
            var xprimeX = xTranspose.Multiply(mobj_X) as DenseMatrix;

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

            mobj_Coefficients = invXprimeXxTrans.Multiply(mobj_Y) as DenseMatrix;

            if (mobj_Coefficients == null)
            {
                throw new InvalidOperationException("Unable to multiple inv_xprime_x and mobj_X in clsMixtureModelRegressionEM::CalculateRegressionFunction");
            }

            var nextCoefficients = new DenseMatrix(mshort_order + 1, 1);
            var bestCoefficients = new DenseMatrix(mshort_order + 1, 1);

            mvect_Likelihoods.Clear();
            mvect_meanNoise.Clear();
            mvect_probReal.Clear();
            mvect_stdevNoise.Clear();
            mvect_stdevReal.Clear();

            double sumY = 0;
            double sumYY = 0;
            for (int ptNum = 0; ptNum < numPts; ptNum++)
            {
                double y = RegressionPoints[ptNum].MdblY;
                sumY += y;
                sumYY += y * y;
            }
            double noiseMean = sumY / numPts;
            double noiseStdev = Math.Sqrt((sumYY - sumY * sumY / numPts) / numPts);

            mvect_meanNoise.Add(noiseMean);
            mvect_stdevNoise.Add(noiseStdev);
            mvect_stdevReal.Add(noiseStdev / 10);
            mvect_probReal.Add(0.5);

            double max_likelihood = -1 * double.MaxValue;
            for (int iterationNum = 0; iterationNum < MAX_ITERATIONS_TO_PERFORM; iterationNum++)
            {
                mint_percent_complete = 20 + (80 * iterationNum) / MAX_ITERATIONS_TO_PERFORM;
                CalculateNextPredictions();
                double likelihood = CalculateLikelihoodsMatrix(iterationNum);
                if (likelihood > max_likelihood)
                {
                    max_likelihood = likelihood;
                    // set the best set of coefficients. 
                    for (short coefficientNum = 0; coefficientNum < mshort_order + 1; coefficientNum++)
                        bestCoefficients.At(coefficientNum, 0, mobj_Coefficients.At(coefficientNum, 0));
                }

                double probability_real = 0;
                CalculateRealProbability(iterationNum, ref probability_real);
                mvect_probReal.Add(probability_real);

                double stdev_real = 0;
                CalculateRealStdev(iterationNum, ref stdev_real);
                mvect_stdevReal.Add(stdev_real);

                double mean_noise = 0, stdev_noise = 0;
                CalculateNoiseStdevAndMean(iterationNum, ref stdev_noise, ref mean_noise);
                mvect_stdevNoise.Add(stdev_noise);
                mvect_meanNoise.Add(mean_noise);

                for (short coefficientNum = 0; coefficientNum < mshort_order + 1; coefficientNum++)
                {
                    double coefficient = 0;
                    CalculateCoefficient(coefficientNum, ref coefficient);
                    nextCoefficients.At(coefficientNum, 0, coefficient);
                }

                // set the next set of coefficient. 
                for (short coefficientNum = 0; coefficientNum < mshort_order + 1; coefficientNum++)
                    mobj_Coefficients.At(coefficientNum, 0, nextCoefficients.At(coefficientNum, 0));
                if (iterationNum > 0)
                {
                    double likelihoodChange = Math.Abs(mvect_Likelihoods[iterationNum] - mvect_Likelihoods[iterationNum - 1]);
                    double minLikelihoodChange = Math.Abs(mdbl_min_percentage_change_to_continue * mvect_Likelihoods[iterationNum - 1]);
                    if (likelihoodChange < minLikelihoodChange)
                        break;
                }
            }

            for(short coefficientNum = 0; coefficientNum < mshort_order + 1; coefficientNum++)
                mobj_Coefficients.At(coefficientNum, 0, bestCoefficients.At(coefficientNum, 0));

            // Calculate Slope
            RegressionResult.Slope = mobj_Coefficients.At(1, 0);
            if (mbln_normalize_input)
            {
                RegressionResult.Slope = RegressionResult.Slope * _normalizingSlopeX / _normalizingSlopeY;
            }
            
            // Calculate Intercept
            RegressionResult.Intercept = mobj_Coefficients.At(0, 0);
            if(mbln_normalize_input)
            {
                double slope = mobj_Coefficients.At(1, 0);
                RegressionResult.Intercept = (RegressionResult.Intercept - _normalizingInterceptY + slope * _normalizingInterceptX) / _normalizingSlopeY; 
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
            var lowerX = minScan;
            var upperX = maxScan - (maxScan - minScan) * percentToIgnore;

            for (int ptNum = 0; ptNum < x.Length; ptNum++)
            {
                var pt = new RegressionPts { MdblX = x[ptNum], MdblY = y[ptNum] };

                if (pt.MdblX > lowerX && pt.MdblX < upperX)
                    RegressionPoints.Add(pt);
            }
        }
    }
}