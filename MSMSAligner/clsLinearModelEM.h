#pragma once
#include "clsRegressionPts.h"
#include "Matrix.h" 
#include <vector> 

namespace RegressionEngine
{
	class clsLinearModelEM
	{
		MATRIX *mobj_X ; 
		MATRIX *mobj_XTranspose ; 
		MATRIX *mobj_Y ; 
		MATRIX *mobj_Weights ; 

		double mdbl_unif_u ;
		double mdbl_stdev ; 
		static const int NUM_ITERATIONS_TO_PERFORM = 20 ; 

		void CalculateProbabilitiesAndWeightMatrix() ; 
		void CalculateSlopeInterceptEstimates() ; 
		double CalculateInitialStdev() ;
	public:
		double mdbl_slope ;
		double mdbl_intercept ; 
		double mdbl_percent_normal ; 
		clsLinearModelEM(void);
		~clsLinearModelEM(void);
		bool CalculateRegressionFunction(std::vector<clsRegressionPts> &vect_pts) ;
		double GetPredictedValue(double x)
		{
			return mdbl_slope * x + mdbl_intercept ; 
		}
	};
}