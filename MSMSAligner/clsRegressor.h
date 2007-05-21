// clsUMCCreator.h
#pragma once
#include "clsLinearModelEM.h"
#include "clsCombinedRegression.h"
#include "clsCentralRegression.h"
#include "clsNaturalCubicSplineRegression.h"
#include <vector> 

using namespace System;
namespace Regressor
{
	public __value enum RegressionType {LINEAR_EM = 0, CUBIC_SPLINE, CENTRAL, HYBRID} ; 
	public __gc class clsRegressor
	{
		RegressionType menmRegressionType ;
		bool mblnSuccessFull ; 
		RegressionEngine::clsLinearModelEM __nogc *mobjLinearRegressionEM ; 
		RegressionEngine::clsCombinedRegression __nogc *mobjCombinedRegression ; 
		RegressionEngine::clsNaturalCubicSplineRegression __nogc *mobjNatCubicRegression ; 
		RegressionEngine::clsCentralRegression __nogc *mobjCentralRegression ; 
		std::vector<RegressionEngine::clsRegressionPts> __nogc *mptrVectRegressionPoints ; 
	public:
		clsRegressor() ; 
		~clsRegressor() ; 


		void SetPoints(float (&x) __gc[], float (&y) __gc[]) ; 
		void PerformRegression(RegressionType type) ; 

		__property double get_Slope()
		{
			return mobjLinearRegressionEM->mdbl_slope ; 
		}
		__property double get_Intercept()
		{
			return mobjLinearRegressionEM->mdbl_intercept ; 
		}

		float GetNETFromScan(float scan) ; 
	};
}
