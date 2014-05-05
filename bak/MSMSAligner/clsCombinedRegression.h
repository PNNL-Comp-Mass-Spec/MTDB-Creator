#pragma once
#include "clsCentralRegression.h" 
#include "clsNaturalCubicSplineRegression.h" 

namespace RegressionEngine
{
	enum enmRegressionType { CENTRAL=0, LSQ, HYBRID };
	class clsCombinedRegression
	{
		enmRegressionType menmRegressionType ; 
		bool mbln_lsq_failed ; 
	public:
		clsCentralRegression mobj_central_regression ; 
		clsNaturalCubicSplineRegression mobj_nat_cubic_regression ; 
		clsCombinedRegression(void);
		~clsCombinedRegression(void);
		void SetCentralRegressionOptions(int num_x_bins, int num_y_bins, int num_jumps, double ztolerance, enmRegressionType reg_type) ; 
		void SetLSQOptions(int num_knots, double outlier_zscore) ; 
		void CalculateRegressionFunction(vector<clsRegressionPts> &calib_matches) ; 
		void PrintRegressionFunction(char *file_name) ; 
		double GetPredictedValue(double x) ; 
	} ;
}