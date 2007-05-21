// This is the main DLL file.

#include "clsRegressor.h"
#using <mscorlib.dll>

namespace Regressor
{

	clsRegressor::clsRegressor()
	{
		mobjLinearRegressionEM = new RegressionEngine::clsLinearModelEM() ; 
		mptrVectRegressionPoints = new std::vector<RegressionEngine::clsRegressionPts> () ;
		mobjCombinedRegression = new RegressionEngine::clsCombinedRegression() ; 
		mobjNatCubicRegression = new RegressionEngine::clsNaturalCubicSplineRegression (); 
		mobjCentralRegression = new RegressionEngine::clsCentralRegression() ; 
		mobjCentralRegression->SetOptions(8, 50, 30, 3) ; 
		mobjCentralRegression->SetOutlierZScore(3) ; 
		mobjNatCubicRegression->SetOptions(8) ; 
		mobjCombinedRegression->SetCentralRegressionOptions(8,50, 30, 3, RegressionEngine::HYBRID) ; 
		mobjCombinedRegression->SetLSQOptions(8, 2.5) ; 
		menmRegressionType = RegressionType::LINEAR_EM ; 
	}

	clsRegressor::~clsRegressor()
	{
	}

	void clsRegressor::SetPoints(float (&x) __gc[], float (&y) __gc[])
	{
		mptrVectRegressionPoints->clear() ; 
		int numPts = x->Length ; 
		double minScan = 1024*1024*16  ; 
		double maxScan = -1 * minScan ; 
		for (int ptNum = 0 ; ptNum < numPts ; ptNum++)
		{
			if (x[ptNum] < minScan)
				minScan = x[ptNum] ; 
			if (x[ptNum] > maxScan)
				maxScan = x[ptNum] ; 
		}

		double percentToIgnore = 0.4 ; 
		double lowerX = minScan ; 
		double upperX = maxScan - (maxScan - minScan) * percentToIgnore ; 

		for (int ptNum = 0 ; ptNum < numPts ; ptNum++)
		{
			RegressionEngine::clsRegressionPts pt ; 
			pt.mdbl_x = (double) x[ptNum] ; 
			pt.mdbl_y = (double) y[ptNum] ; 
			if (pt.mdbl_x > lowerX && pt.mdbl_x < upperX)
				mptrVectRegressionPoints->push_back(pt) ; 
		}
	}

	void clsRegressor::PerformRegression(RegressionType type)
	{
		menmRegressionType = type ; 
		int numIterations = 40 ; 
		switch (menmRegressionType)
		{
			case Regressor::CENTRAL:
				mobjCentralRegression->CalculateRegressionFunction(*mptrVectRegressionPoints);
				break ; 
			case Regressor::LINEAR_EM:
				mobjLinearRegressionEM->CalculateRegressionFunction(*mptrVectRegressionPoints);
				break ; 
			case Regressor::CUBIC_SPLINE:
				mobjNatCubicRegression->CalculateLSQRegressionCoefficients(*mptrVectRegressionPoints, numIterations);
				break ; 
			case Regressor::HYBRID:
				mobjCombinedRegression->CalculateRegressionFunction(*mptrVectRegressionPoints);
				break ; 
			default:
				break ; 

		}
	}

	float clsRegressor::GetNETFromScan(float scan)
	{
		switch (menmRegressionType)
		{
			case Regressor::CENTRAL:
				return mobjCentralRegression->GetPredictedValue(scan);
				break ; 
			case Regressor::LINEAR_EM:
				return mobjLinearRegressionEM->GetPredictedValue(scan);
				break ; 
			case Regressor::CUBIC_SPLINE:
				return mobjNatCubicRegression->GetPredictedValue(scan);
				break ; 
			case Regressor::HYBRID:
				return mobjCombinedRegression->GetPredictedValue(scan);
				break ; 
			default:
				break ; 

		}
	}


}
