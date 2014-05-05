#pragma once
#include "clsRegressionPts.h"
#include "Matrix.h" 
#include <vector> 

namespace RegressionEngine
{
	class clsMixtureModelRegressionEM
	{
		double mdbl_normalizing_slope_x ; 
		double mdbl_normalizing_slope_y ; 
		double mdbl_normalizing_intercept_x ; 
		double mdbl_normalizing_intercept_y ; 

		double mdbl_min_percentage_change_to_continue ; 
		bool mbln_normalize_input ; 

		MATRIX *mobj_X ; 
		MATRIX *mobj_YRegressed ; 
		MATRIX *mobj_Y ; 

		MATRIX *mobj_Likelihoods ; 
		MATRIX *mobj_Coefficients ; 

		std::vector<double> mvect_stdevReal ;
		std::vector<double> mvect_stdevNoise ;
		std::vector<double> mvect_meanNoise ;
		std::vector<double> mvect_probReal ; 
		std::vector<double> mvect_Likelihoods ; 
		
		short mshort_order ; 

		int mint_percent_complete ; 

		static const int MAX_ITERATIONS_TO_PERFORM = 250 ; 

		void SetupMatrices(std::vector<clsRegressionPts> &vect_pts) ; 

		void CalculateCoefficient(short coeffNum, double &coeff) ; 
		void CalculateNextPredictions() ; 

		void CalculateNoiseStdevAndMean(int iterationNum, double &stdev, double &mean) ;  
		void CalculateRealStdev(int iterationNum, double &stdev) ;  
		void CalculateRealProbability(int iterationNum, double &probability) ;  
		// returns log likelihood.
		double CalculateLikelihoodsMatrix(int iterationNum) ; 
		void FreeMatrices() ; 
		void NormalizeXAndYs(std::vector<clsRegressionPts> &vect_pts) ; 
		void CopyWithoutXDuplicates(std::vector<clsRegressionPts> &vect_pts, 
			std::vector<clsRegressionPts> &vectToCopyTo) ; 
	public:
		static const double PI ; 
		static const double SQRT2PI ; 

		clsMixtureModelRegressionEM(void);
		~clsMixtureModelRegressionEM(void);
		void CalculateRegressionFunction(std::vector<clsRegressionPts> &vect_ptsIn) ;

		double GetPredictedValue(double x) ; 
		int PercentComplete() ; 
		double GetSlope() ; 
		double GetIntercept() ; 
		short GetRegressionOrder() ; 
		void SetRegressionOrder(short order) ;
	};
}