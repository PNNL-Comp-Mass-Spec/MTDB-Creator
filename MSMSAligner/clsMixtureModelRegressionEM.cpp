
#pragma once 
#pragma managed(push)
#pragma unmanaged 

#include ".\clsMixtureModelRegressionEM.h"
#include <float.h>
#include <math.h>
#include <iostream> 
#include <algorithm>
#include <fstream> 
namespace RegressionEngine
{
	const double RegressionEngine::clsMixtureModelRegressionEM::PI = 3.14159265358979323846 ; 
	const double RegressionEngine::clsMixtureModelRegressionEM::SQRT2PI = 2.506628274631 ; 

	bool CompareRegressionPointsByX(clsRegressionPts &a, clsRegressionPts &b)
	{
		return a.mdbl_x < b.mdbl_x ; 
	}
	
	bool AreRegressionElementsXEqual(clsRegressionPts &a, clsRegressionPts &b)
	{
		return (a.mdbl_x == b.mdbl_x) ; 
	}

	clsMixtureModelRegressionEM::clsMixtureModelRegressionEM(void)
	{
		mobj_X = NULL ; 
		mobj_Y = NULL ; 
		mobj_Coefficients = NULL ; 
		mobj_Likelihoods = NULL ; 
		mobj_YRegressed = NULL ; 
		mint_percent_complete = 0 ; 
		mshort_order = 1 ; 
		mbln_normalize_input = true ; 
		mdbl_min_percentage_change_to_continue = 0.0001 ; 
	}

	clsMixtureModelRegressionEM::~clsMixtureModelRegressionEM(void)
	{
		FreeMatrices() ; 
	}

	void clsMixtureModelRegressionEM::FreeMatrices()
	{
		if (mobj_X != NULL)
			matrix_free(mobj_X) ;
		if (mobj_Y != NULL)
			matrix_free(mobj_Y) ; 
		if (mobj_Coefficients != NULL)
			matrix_free(mobj_Coefficients) ; 
		if (mobj_Likelihoods != NULL)
			matrix_free(mobj_Likelihoods) ; 
		if (mobj_YRegressed != NULL)
			matrix_free(mobj_YRegressed) ; 
	}

	void clsMixtureModelRegressionEM::SetupMatrices(std::vector<clsRegressionPts> &vect_pts)
	{
		FreeMatrices() ; 
		int numPoints = vect_pts.size() ; 
		mobj_X = matrix_allocate(numPoints, mshort_order+1, sizeof(double)) ; 
		mobj_Y = matrix_allocate(numPoints, 1, sizeof(double)) ; 
		mobj_YRegressed = matrix_allocate(numPoints, 1, sizeof(double)) ; 
		mobj_Likelihoods = matrix_allocate(numPoints, 2, sizeof(double)) ; 
		//mobj_Coefficients = matrix_allocate(mshort_order+1, 1, sizeof(double)) ; 


		// set up X, Y and weights vector. 
		double **ptrMatrixX = (double **) mobj_X->ptr ; 
		double **ptrMatrixY = (double **) mobj_Y->ptr ; 

		for (int index = 0 ; index < numPoints ; index++)
		{
			clsRegressionPts currentPoint = vect_pts[index] ; 
			double val = 1; 
			for (int coeffNum = 0 ; coeffNum < mshort_order + 1 ; 
				coeffNum++)
			{
				ptrMatrixX[index][coeffNum] = val ; 
				val *= currentPoint.mdbl_x ; 
			}

			ptrMatrixY[index][0] = currentPoint.mdbl_y ; 
		}
	}

	void clsMixtureModelRegressionEM::NormalizeXAndYs(std::vector<clsRegressionPts> &vect_pts)
	{
		int numPoints = vect_pts.size() ; 
		if (numPoints == 0)
			throw "Zero elements passed to NormalizeXAndYs" ; 

		std::vector<double> vectXs ; 
		std::vector<double> vectYs ; 
		vectXs.reserve(numPoints) ; 
		vectYs.reserve(numPoints) ; 

		for (int index = 0 ; index < numPoints ; index++)
		{
			clsRegressionPts currentPoint = vect_pts[index] ; 
			vectXs.push_back(currentPoint.mdbl_x) ; 
			vectYs.push_back(currentPoint.mdbl_y) ; 
		}
		std::sort(vectXs.begin(), vectXs.end()) ; 
		std::sort(vectYs.begin(), vectYs.end()) ; 

		int medianIndex = 0 ; 
		if (numPoints % 2 == 0)
		{
			medianIndex = (numPoints-1)/2 ;
		}
		else 
		{
			medianIndex = numPoints/2 ; 
		}

		double xMedian = vectXs[medianIndex] ; 
		double yMedian = vectYs[medianIndex] ; 
		double xMin = vectXs[0] ; 
		double xMax = vectXs[numPoints-1] ; 
		double yMin = vectYs[0] ; 
		double yMax = vectYs[numPoints-1] ; 

		mdbl_normalizing_intercept_x = - xMedian/(xMax-xMin) ; 
		mdbl_normalizing_slope_x = 1/(xMax-xMin) ; 

		mdbl_normalizing_intercept_y = - yMedian/(yMax-yMin) ; 
		mdbl_normalizing_slope_y = 1/(yMax-yMin) ; 

		for (int index = 0 ; index < numPoints ; index++)
		{
			vect_pts[index].mdbl_x = vect_pts[index].mdbl_x * mdbl_normalizing_slope_x + mdbl_normalizing_intercept_x ; 
			vect_pts[index].mdbl_y = vect_pts[index].mdbl_y * mdbl_normalizing_slope_y + mdbl_normalizing_intercept_y ; 
		}

	}

	double clsMixtureModelRegressionEM::GetPredictedValue(double x)
	{
		if (mbln_normalize_input)
			x = x * mdbl_normalizing_slope_x + mdbl_normalizing_intercept_x ; 
		double val = 1; 
		double interpolatedVal = 0 ; 
		double **ptrMatrixCoeffs = (double **) mobj_Coefficients->ptr; 

		for (int coeffNum = 0 ; coeffNum < mshort_order + 1 ; 
			coeffNum++)
		{
			interpolatedVal += val * ptrMatrixCoeffs[coeffNum][0] ; 
			val *= x ; 
		}
		if (mbln_normalize_input)
		{
			interpolatedVal = (interpolatedVal - mdbl_normalizing_intercept_y)/mdbl_normalizing_slope_y ; 
		}
		return interpolatedVal ; 
	}

	void clsMixtureModelRegressionEM::CalculateCoefficient(short coeffNum, double &coeff)
	{
		int numPoints = mobj_X->rows ; 

		double **ptrMatrixCoeffs = (double **) mobj_Coefficients->ptr; 
		double **ptrMatrixX = (double **) mobj_X->ptr ; 
		double **ptrMatrixY = (double **) mobj_Y->ptr ; 
		double **ptrMatrixYPrime = (double **) mobj_YRegressed->ptr ; 
		double **ptrMatrixLikelihoods = (double **) mobj_Likelihoods->ptr ; 

		double numerator = 0 ; 
		double denominator = 0 ; 

		for (int index = 0 ; index < numPoints ; index++)
		{
			double diff = ptrMatrixY[index][0] - ptrMatrixYPrime[index][0] ;  
			double missingX = ptrMatrixX[index][coeffNum] ; 
			double missing_term =  missingX * ptrMatrixCoeffs[coeffNum][0] ;
			diff += missing_term ; 

			double likelihood_real = ptrMatrixLikelihoods[index][0] ; 
			double likelihood_noise = ptrMatrixLikelihoods[index][1] ; 
			double probability = likelihood_real/ (likelihood_noise + likelihood_real)  ; 

			numerator += diff * missingX * probability ;
			denominator += probability * missingX * missingX ; 
		}
		coeff = numerator/denominator ; 
	}

	void clsMixtureModelRegressionEM::CalculateNoiseStdevAndMean(int iterationNum, double &stdev, double &mean)
	{
		int numPoints = mobj_X->rows ; 
		double **ptrMatrixY = (double **) mobj_Y->ptr ; 
		double **ptrMatrixYPrime = (double **) mobj_YRegressed->ptr ; 
		double **ptrMatrixLikelihoods = (double **) mobj_Likelihoods->ptr ; 

		double sum_sigma_sq_w = 0 ; 
		double sum_w = 0 ; 
		double sum_w_noise = 0 ; 
		double noise_mean = mvect_meanNoise[iterationNum] ; 

		for (int index = 0 ; index < numPoints ; index++)
		{
			double y = ptrMatrixY[index][0] ;  
			double likelihood_real = ptrMatrixLikelihoods[index][0] ; 
			double likelihood_noise = ptrMatrixLikelihoods[index][1] ; 
			double probability = likelihood_noise/ (likelihood_noise + likelihood_real)  ; 

			sum_sigma_sq_w += probability * (y - noise_mean) * (y - noise_mean) ; 
			sum_w_noise += y * probability ; 
			sum_w += probability ; 
		}
		mean = sum_w_noise/sum_w ; 
		stdev = sqrt(sum_sigma_sq_w/sum_w) ; 
	}

	void clsMixtureModelRegressionEM::CalculateRealStdev(int iterationNum, double &stdev)
	{
		int numPoints = mobj_X->rows ; 
		double **ptrMatrixY = (double **) mobj_Y->ptr ; 
		double **ptrMatrixYPrime = (double **) mobj_YRegressed->ptr ; 
		double **ptrMatrixLikelihoods = (double **) mobj_Likelihoods->ptr ; 

		double sum_sigma_sq_w = 0 ; 
		double sum_w = 0 ; 
		for (int index = 0 ; index < numPoints ; index++)
		{
			double diff = ptrMatrixY[index][0] - ptrMatrixYPrime[index][0] ;  
			double likelihood_real = ptrMatrixLikelihoods[index][0] ; 
			double likelihood_noise = ptrMatrixLikelihoods[index][1] ; 
			double probability = likelihood_real/ (likelihood_noise + likelihood_real)  ; 
			sum_sigma_sq_w += probability * diff * diff ; 
			sum_w += probability ; 
		}
		stdev = sqrt(sum_sigma_sq_w/sum_w) ; 
	}

	void clsMixtureModelRegressionEM::CalculateRealProbability(int iterationNum, double &probability)
	{
		int numPoints = mobj_X->rows ; 
		double **ptrMatrixLikelihoods = (double **) mobj_Likelihoods->ptr ; 

		double normal_prob_sum = 0 ; 

		for (int index = 0 ; index < numPoints ; index++)
		{
			double likelihood_real = ptrMatrixLikelihoods[index][0] ; 
			double likelihood_noise = ptrMatrixLikelihoods[index][1] ; 
			normal_prob_sum +=  likelihood_real/ (likelihood_noise + likelihood_real) ; 
		}
		normal_prob_sum /= numPoints ; 
		probability = normal_prob_sum ; 

	}

	void clsMixtureModelRegressionEM::CalculateNextPredictions()
	{
		mobj_YRegressed = matrix_mult(mobj_X, mobj_Coefficients) ; 
	}

	double clsMixtureModelRegressionEM::CalculateLikelihoodsMatrix(int iterationNum)
	{
		int numPoints = mobj_X->rows ; 
		double **ptrMatrixX = (double **) mobj_X->ptr ; 
		double **ptrMatrixY = (double **) mobj_Y->ptr ; 
		double **ptrMatrixYPrime = (double **) mobj_YRegressed->ptr ; 
		double **ptrMatrixLikelihoods = (double **) mobj_Likelihoods->ptr ; 

		double noise_sigma = mvect_stdevNoise[iterationNum] ; 
		double noise_mean = mvect_meanNoise[iterationNum] ; 
		double real_sigma = mvect_stdevReal[iterationNum] ; 
		double probability_real = mvect_probReal[iterationNum] ; 

		double log_likelihood = 0 ; 
		for (int index = 0 ; index < numPoints ; index++)
		{
			double diff = ptrMatrixY[index][0] - ptrMatrixYPrime[index][0] ;  
			double standardizedNoise = (diff - noise_mean) / noise_sigma ; 
			double standardizedReal = diff / real_sigma ; 

			double likelihood_real = probability_real * exp(-1 * standardizedReal * standardizedReal) / (SQRT2PI * real_sigma) ; 
			double likelihood_noise = (1-probability_real) * exp(-1 * standardizedNoise * standardizedNoise) / (SQRT2PI * noise_sigma) ; 

			ptrMatrixLikelihoods[index][0] = likelihood_real ; 
			ptrMatrixLikelihoods[index][1] = likelihood_noise ; 
			log_likelihood += log(likelihood_real + likelihood_noise) ; 
		}
		mvect_Likelihoods.push_back(log_likelihood) ; 
		return log_likelihood ; 
	}


	void clsMixtureModelRegressionEM::CalculateRegressionFunction(std::vector<clsRegressionPts> &vect_ptsIn)
	{

		std::vector<clsRegressionPts> vect_pts ; 
		CopyWithoutXDuplicates(vect_ptsIn, vect_pts) ; 
		int numPts = vect_pts.size() ; 

		if (mbln_normalize_input)
			NormalizeXAndYs(vect_pts) ; 
		mint_percent_complete = 0 ; 
		SetupMatrices(vect_pts) ;
		mint_percent_complete = 10 ; 

		// CALCULATE FIRST SET OF COEFFICIENTS HERE .. 
		// Remember that the coefficients should be: 
		// (XX')-1X 
		//PrintMatrix(mobj_X, "c:\\x_matrix.csv") ; 

		MATRIX *XTranspose = matrix_transpose(mobj_X) ; 
		MATRIX *xprime_x = matrix_mult(XTranspose, mobj_X) ; 
		if (xprime_x == NULL)
		{
			matrix_free(XTranspose) ; 
			throw "Unable to multiply mobj_X with XTranspose in clsMixtureModelRegressionEM::CalculateRegressionFunction" ; 
		}
		//PrintMatrix(xprime_x, "c:\\test_matrix.csv") ; 
		MATRIX *inv_xprime_x = matrix_invert(xprime_x) ; 
		matrix_free(xprime_x) ; 
		if (inv_xprime_x == NULL)
		{
			matrix_free(XTranspose) ; 
			throw "Unable to invert xprime_x in clsMixtureModelRegressionEM::CalculateRegressionFunction" ; 
		}
		MATRIX *inv_xprime_x_xTrans = matrix_mult(inv_xprime_x, XTranspose) ; 
		matrix_free(inv_xprime_x) ; 	
		matrix_free(XTranspose) ; 
		if (inv_xprime_x_xTrans == NULL)
		{
			throw "Unable to multiple inv_xprime_wx and mobj_X in clsMixtureModelRegressionEM::CalculateRegressionFunction" ; 
		}
		mobj_Coefficients = matrix_mult(inv_xprime_x_xTrans, mobj_Y) ; 
		matrix_free(inv_xprime_x_xTrans) ; 	
		if (mobj_Coefficients == NULL)
		{
			throw "Unable to multiple inv_xprime_x and mobj_X in clsMixtureModelRegressionEM::CalculateRegressionFunction" ; 
		}

		MATRIX *next_coefficients = matrix_allocate(mshort_order+1, 1, sizeof(double)) ; 
		MATRIX *best_coefficients = matrix_allocate(mshort_order+1, 1, sizeof(double)) ; 

		double **ptr_next_coeffs = (double **) next_coefficients->ptr ; 
		double **ptr_best_coeffs = (double **) best_coefficients->ptr ; 
		double **ptr_current_coeffs = (double **) mobj_Coefficients->ptr ; 

		mint_percent_complete = 20 ; 

		mvect_Likelihoods.clear() ; 
		mvect_meanNoise.clear() ; 
		mvect_probReal.clear() ; 
		mvect_stdevNoise.clear() ; 
		mvect_stdevReal.clear() ; 

		double sumY = 0 ; 
		double sumYY = 0 ; 
		for (int ptNum = 0 ; ptNum < numPts ; ptNum++)
		{
			double y = vect_pts[ptNum].mdbl_y ; 
			sumY += y ; 
			sumYY += y * y ; 
		}
		double noiseMean = sumY / numPts ; 
		double noiseStdev = sqrt((sumYY - sumY * sumY/numPts)/numPts) ; 

		mvect_meanNoise.push_back(noiseMean) ; 
		mvect_stdevNoise.push_back(noiseStdev) ; 
		//mvect_stdevReal.push_back(0.05) ; 
		mvect_stdevReal.push_back(noiseStdev/10) ; 
		mvect_probReal.push_back(0.5) ; 


//		std::ostream& fout = std::cout ; 
		bool printCoeffs = true ; 
		std::ostream& fout= std::cout ; 

		bool last_likelihood = 0 ; 
		double max_likelihood = -1 * DBL_MAX ;
		for (int iterationNum = 0 ; iterationNum < MAX_ITERATIONS_TO_PERFORM ; iterationNum++)
		{
			mint_percent_complete = 20 + (80 * iterationNum)/MAX_ITERATIONS_TO_PERFORM ; 
			CalculateNextPredictions() ; 
			double likelihood = CalculateLikelihoodsMatrix(iterationNum) ;
			if (likelihood > max_likelihood)
			{
				max_likelihood = likelihood ; 
				// set the best set of coefficients. 
				for (short coefficientNum = 0 ; coefficientNum < mshort_order + 1 ; 
					coefficientNum++)
					ptr_best_coeffs[coefficientNum][0] = ptr_current_coeffs[coefficientNum][0] ;
			}

			double probability_real = 0 ; 
			CalculateRealProbability(iterationNum, probability_real) ; 
			mvect_probReal.push_back(probability_real) ; 

			double stdev_real = 0 ; 
			CalculateRealStdev(iterationNum, stdev_real) ; 
			mvect_stdevReal.push_back(stdev_real) ; 

			double mean_noise = 0, stdev_noise = 0 ; 
			CalculateNoiseStdevAndMean(iterationNum, stdev_noise, mean_noise) ; 
			mvect_stdevNoise.push_back(stdev_noise) ; 
			mvect_meanNoise.push_back(mean_noise) ; 

			/*
			if (printCoeffs)
			{
				fout<<"Stdev Real = "<< stdev_real<<" Mean noise = "<<mean_noise ; 
				fout<<" Stdev Noise = "<<stdev_noise<<" Prob real = "<<probability_real ; 
				fout<<" Likelihood = "<<mvect_Likelihoods[mvect_Likelihoods.size()-1]<<std::endl ; 
			}
			*/

			for (short coefficientNum = 0 ; coefficientNum < mshort_order + 1 ; 
				coefficientNum++)
			{
				double coefficient = 0 ; 
				CalculateCoefficient(coefficientNum, coefficient) ; 
				ptr_next_coeffs[coefficientNum][0] = coefficient;
				/*
				if (printCoeffs)
					fout<<coefficient<<"\t" ; 
				*/
			}

			/*
			if (printCoeffs)
				fout<<std::endl ; 
			*/

			// set the next set of coefficient. 
			for (short coefficientNum = 0 ; coefficientNum < mshort_order + 1 ; 
				coefficientNum++)
				 ptr_current_coeffs[coefficientNum][0] = ptr_next_coeffs[coefficientNum][0] ;
			if (iterationNum > 0)
			{
				double likelihoodChange = abs(mvect_Likelihoods[iterationNum] - mvect_Likelihoods[iterationNum-1]) ; 
				double minLikelihoodChange	= abs(mdbl_min_percentage_change_to_continue * mvect_Likelihoods[iterationNum-1]) ; 
				if (likelihoodChange < minLikelihoodChange)
					break ; 
			}
		}
		for (short coefficientNum = 0 ; coefficientNum < mshort_order + 1 ; 
			coefficientNum++)
			ptr_current_coeffs[coefficientNum][0] = ptr_best_coeffs[coefficientNum][0] ;
		matrix_free(next_coefficients) ; 
		matrix_free(best_coefficients) ; 
	}


	void clsMixtureModelRegressionEM::CopyWithoutXDuplicates(std::vector<clsRegressionPts> &vect_pts, 
		std::vector<clsRegressionPts> &vectToCopyTo)
	{
		std::sort(vect_pts.begin(), vect_pts.end(), &CompareRegressionPointsByX) ; 
		vectToCopyTo.insert(vectToCopyTo.begin(), vect_pts.begin(), vect_pts.end()) ; 
		vectToCopyTo.erase(std::unique(vectToCopyTo.begin(), vectToCopyTo.end(),&AreRegressionElementsXEqual), vectToCopyTo.end()) ; 
		//vectToCopyTo.resize(iter - vectToCopyTo.begin()) ; 
	}

	int clsMixtureModelRegressionEM::PercentComplete()
	{
		return mint_percent_complete ; 
	}
	double clsMixtureModelRegressionEM::GetSlope()
	{
		if (mobj_Coefficients == NULL)
			return 0 ; 
		double **ptr_current_coeffs = (double **) mobj_Coefficients->ptr ; 
		double slope = ptr_current_coeffs[1][0] ; 
		if (mbln_normalize_input)
		{
			slope = slope * mdbl_normalizing_slope_x / mdbl_normalizing_slope_y ; 
		}
		return slope ; 
	}

	double clsMixtureModelRegressionEM::GetIntercept()
	{
		if (mobj_Coefficients == NULL)
			return 0 ; 

		double **ptr_current_coeffs = (double **) mobj_Coefficients->ptr ; 
		double intercept = ptr_current_coeffs[0][0] ; 
		if (mbln_normalize_input)
		{
			double slope = ptr_current_coeffs[1][0] ; 
			// need to transform intercept into the scale input by caller.
			intercept = (intercept - mdbl_normalizing_intercept_y + slope * mdbl_normalizing_intercept_x)/mdbl_normalizing_slope_y ; 
		}
		return  intercept ; 
	}

	short clsMixtureModelRegressionEM::GetRegressionOrder()
	{
		return mshort_order ; 
	}

	void clsMixtureModelRegressionEM::SetRegressionOrder(short order)
	{
		mshort_order = order ; 
	}


}

#pragma managed(pop)