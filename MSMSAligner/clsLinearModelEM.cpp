

#pragma once 
#pragma managed(push)
#pragma unmanaged 

#include ".\clslinearmodelem.h"
#include <float.h>
#include <math.h>
#include <iostream> 
const double PI = 3.1415926 ; 

namespace RegressionEngine
{
	clsLinearModelEM::clsLinearModelEM(void)
	{
		mobj_X = NULL ; 
		mobj_Y = NULL ; 
		mobj_Weights = NULL ; 
		mint_percent_complete = 0 ; 
	}

	clsLinearModelEM::~clsLinearModelEM(void)	
	{
		if (mobj_X != NULL)
		{
			matrix_free(mobj_X) ;
			matrix_free(mobj_Y) ; 
			matrix_free(mobj_Weights) ; 
			matrix_free(mobj_XTranspose) ; 
		}
	}


	void clsLinearModelEM::CalculateProbabilitiesAndWeightMatrix()
	{

		int numPoints = mobj_X->rows ; 
		double **ptrMatrixWeights = (double **) mobj_Weights->ptr ; 
		double **ptrMatrixX = (double **) mobj_X->ptr ; 
		double **ptrMatrixY = (double **) mobj_Y->ptr ; 

		double normal_prob_sum = 0 ; 
		double sum_square_diff = 0 ;
		double sum_square_diff_for_rsq = 0 ;
		double sum_square_y = 0 ; 
		double sum_y = 0 ; 
		for (int index = 0 ; index < numPoints ; index++)
		{
			double diff = ptrMatrixY[index][0] - (mdbl_slope * ptrMatrixX[index][0] + mdbl_intercept)  ;
			double exponent = exp(-0.5*(diff/mdbl_stdev)*(diff/mdbl_stdev)) ; 
			double normalizer = mdbl_stdev*sqrt(2*PI) ;  
			double prob_normal_conditional = exponent/normalizer; 
			double prob_normal_posterior = (mdbl_percent_normal * prob_normal_conditional) / (mdbl_percent_normal * prob_normal_conditional + (1-mdbl_percent_normal)* mdbl_unif_u) ; 
			ptrMatrixWeights[index][index] = prob_normal_posterior ;
			normal_prob_sum += prob_normal_posterior ; 
			sum_square_diff += prob_normal_posterior * diff * diff ; 
			sum_square_diff_for_rsq += diff * diff ;
			sum_square_y += (ptrMatrixY[index][0]*ptrMatrixY[index][0]) ; 
			sum_y += ptrMatrixY[index][0] ; 
		}
		mdbl_percent_normal = normal_prob_sum/numPoints ; 
		mdbl_stdev = sqrt(sum_square_diff / (normal_prob_sum)) ; 
		double yVar = sum_square_y - sum_y * sum_y/numPoints ; 
		if (yVar == 0)
		{
			mdbl_rsq = 0 ; 
			throw "All y values are the same" ;
		}
		else
		{
			mdbl_rsq = 1 - sum_square_diff_for_rsq/yVar ; 
			//mdbl_rsq = 1 - sum_square_diff/yVar ; 
		}
	}

	void clsLinearModelEM::CalculateSlopeInterceptEstimates()
	{
		MATRIX *wx = matrix_mult(mobj_Weights, mobj_X) ; 
		if (wx == NULL)
		{
			///PrintMatrix(wx, "c:\\test1.csv") ; 
		}
		MATRIX *xprime_wx = matrix_mult(mobj_XTranspose, wx) ; 
		if (xprime_wx == NULL)
		{
			//PrintMatrix(xprime_wx, "c:\\test1.csv") ; 
		}
		MATRIX *inv_xprime_wx = matrix_invert(xprime_wx) ; 
		if (inv_xprime_wx == NULL)
		{
			//PrintMatrix(inv_xprime_wx, "c:\\test1.csv") ; 
		}
		MATRIX *wy = matrix_mult(mobj_Weights, mobj_Y) ; 
		if (wy == NULL)
		{
			//PrintMatrix(wy, "c:\\test1.csv") ; 
		}
		MATRIX *xprime_wy = matrix_mult(mobj_XTranspose, wy) ; 
		if (xprime_wy == NULL)
		{
			//PrintMatrix(xprime_wy, "c:\\test1.csv") ; 
		}
		MATRIX *beta = matrix_mult(inv_xprime_wx, xprime_wy) ; 
		if (beta == NULL)
		{
			//PrintMatrix(beta, "c:\\test1.csv") ; 
		}

		double **betaPtr = (double **) beta->ptr ; 
		mdbl_slope = betaPtr[0][0] ; 
		mdbl_intercept = betaPtr[1][0] ; 
		int numPoints = mobj_X->rows ; 
		double maxDiff = -1*DBL_MAX ; 
		double minDiff = DBL_MAX ; 
		double **ptrMatrixX = (double **) mobj_X->ptr ; 
		double **ptrMatrixY = (double **) mobj_Y->ptr ; 

		double maxY = -1 * DBL_MAX ; 
		for (int index = 0 ; index < numPoints ; index++)
		{
			double diff = ptrMatrixY[index][0] - (mdbl_slope * ptrMatrixX[index][0] + mdbl_intercept)  ;
			if (diff > maxDiff)
				maxDiff = diff ; 
			if (diff < minDiff)
				minDiff = diff ; 
			if (ptrMatrixY[index][0] > maxY) 
				maxY = ptrMatrixY[index][0] ; 
		}

		mdbl_unif_u = 1.0 / (maxDiff - minDiff) ; 
		mdbl_unif_u = 1.0 / maxY ; 
		matrix_free(wx) ;
		matrix_free(xprime_wx) ;
		matrix_free(inv_xprime_wx) ;
		matrix_free(wy) ; 
		matrix_free(xprime_wy) ; 
		matrix_free(beta) ; 
	}


	double clsLinearModelEM::CalculateInitialStdev()
	{
		double **ptrMatrixX = (double **) mobj_X->ptr ; 
		double **ptrMatrixY = (double **) mobj_Y->ptr ; 
		int numPoints = mobj_X->rows ; 

		mdbl_stdev = 0 ;
		for (int index = 0 ; index < numPoints ; index++)
		{
			double diff = ptrMatrixY[index][0] - (mdbl_slope * ptrMatrixX[index][0] + mdbl_intercept)  ;
			mdbl_stdev += diff * diff ; 
		}
		mdbl_stdev /= numPoints ; 
		mdbl_stdev = sqrt(mdbl_stdev); 
		return mdbl_stdev ; 
	}

	bool clsLinearModelEM::CalculateRegressionFunction(std::vector<clsRegressionPts> &vectPoints)
	{
		mint_percent_complete = 0 ; 
		int numPoints = vectPoints.size() ; 
		if (mobj_X != NULL)
		{
			matrix_free(mobj_X) ;
			matrix_free(mobj_Y) ; 
			matrix_free(mobj_Weights) ; 
			matrix_free(mobj_XTranspose) ; 
		}

		mobj_X = matrix_allocate(numPoints, 2, sizeof(double)) ; 
		mobj_Y = matrix_allocate(numPoints, 1, sizeof(double)) ; 
		mobj_Weights = matrix_allocate(numPoints, numPoints, sizeof(double)) ; 

		mdbl_percent_normal = 0.5 ; 

		// set up X, Y and weights vector. 
		double **ptrMatrixX = (double **) mobj_X->ptr ; 
		double **ptrMatrixY = (double **) mobj_Y->ptr ; 
		double **ptrMatrixWeights = (double **) mobj_Weights->ptr ; 

		double sumX = 0 ; 
		double sumXX = 0 ;

		for (int index = 0 ; index < numPoints ; index++)
		{
			clsRegressionPts currentPoint = vectPoints[index] ; 

			ptrMatrixX[index][0] = currentPoint.mdbl_x ; 
			sumX += currentPoint.mdbl_x ; 
			sumXX += (currentPoint.mdbl_x * currentPoint.mdbl_x ); 
			ptrMatrixX[index][1] = 1 ; 

			ptrMatrixY[index][0] = currentPoint.mdbl_y ; 
			for (int colNum = 0 ; colNum < numPoints ; colNum++)
			{
				ptrMatrixWeights[index][colNum] = 0 ; 
			}
			ptrMatrixWeights[index][index] = 1 ; 
			mint_percent_complete = (10 * index)/numPoints ; 
		}

		if (sumX * sumX == numPoints * sumXX)
		{
			mdbl_intercept = 0 ; 
			mdbl_slope = 0 ; 
			throw "All X values cannot be same for linear regression" ; 
		}

		mobj_XTranspose = matrix_transpose(mobj_X) ; 
		mint_percent_complete = 20 ; 

		CalculateSlopeInterceptEstimates(); 
		CalculateInitialStdev() ; 

		// matrices are set up. 
		for (int iterationNum = 0 ; iterationNum < NUM_ITERATIONS_TO_PERFORM ; iterationNum++)
		{
			mint_percent_complete = 20 + (80*(iterationNum+1))/NUM_ITERATIONS_TO_PERFORM ; 
			CalculateSlopeInterceptEstimates() ; 
			CalculateProbabilitiesAndWeightMatrix() ; 
			if (mdbl_percent_normal < 0.01)
				return false ; 
		}
		return true ; 
	}
	int clsLinearModelEM::PercentComplete()
	{
		return mint_percent_complete ; 
	}
}
#pragma managed(pop)