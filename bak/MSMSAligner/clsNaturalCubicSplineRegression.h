#pragma once
#include "clsRegressionPts.h"
#include "Matrix.h" 
#include <vector>
using namespace std ; 

namespace RegressionEngine
{
	class clsNaturalCubicSplineRegression
	{

		vector<clsRegressionPts> mvect_pts ; 
		vector<double> mvect_interval_start ; 

		void PreprocessCopyData(vector<clsRegressionPts> vect_pts) ; 
		void Clear() ; 
	public:
		int mint_num_knots ;
		double mdbl_min_x ; 
		double mdbl_max_x ; 
		double marr_coeffs[512] ; 
		clsNaturalCubicSplineRegression(void): mint_num_knots(2){} ;
		~clsNaturalCubicSplineRegression(void) {} ;
		void SetOptions(int num_knots) { mint_num_knots = num_knots ; } ; 
		bool CalculateLSQRegressionCoefficients(vector<clsRegressionPts> &vect_pts) ;
		bool CalculateLSQRegressionCoefficients(vector<clsRegressionPts> &vect_pts, int numIterations) ;
		void PrintRegressionFunction(char *file_name) ;
		double GetPredictedValue(double x) ; 
		void PrintPoints(char *file_name) ; 
	};
}