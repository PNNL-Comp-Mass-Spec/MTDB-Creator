// This is the main DLL file.

#include "clsRegressor.h"
#using <mscorlib.dll>

namespace Regressor
{

	clsRegressor::clsRegressor()
	{
		mobjLinearRegressionEM   = new RegressionEngine::clsLinearModelEM() ; 
		mobjMixtureRegressionEM  = new RegressionEngine::clsMixtureModelRegressionEM() ; 
		mptrVectRegressionPoints = new std::vector<RegressionEngine::clsRegressionPts> () ;
		menmRegressionType       = MIXTURE_REGRESSION ; 
	}

	clsRegressor::~clsRegressor()
	{
		if (mobjLinearRegressionEM != NULL)
		{
			delete mobjLinearRegressionEM ; 
			mobjLinearRegressionEM = NULL ; 
		}
		if (mobjMixtureRegressionEM != NULL)
		{
			delete mobjMixtureRegressionEM ; 
			mobjMixtureRegressionEM = NULL ; 
		}
		if (mptrVectRegressionPoints != NULL) 
		{
			delete mptrVectRegressionPoints ; 
			mptrVectRegressionPoints = NULL ; 
		}
	}

	//void clsRegressor::SetPoints(array<float> ^(&x) __gc[], float (&y) __gc[])
	void clsRegressor::SetPoints(array<float> ^ %x, array<float> ^% y)
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
		mobjLinearRegressionEM->mdbl_slope = 0 ; 
		mobjLinearRegressionEM->mdbl_intercept = 0 ; 

		if (!mptrVectRegressionPoints || mptrVectRegressionPoints->size() <2)
		{
			return ; 
		}
		

		menmRegressionType = type ; 
		int numIterations = 40 ; 
		bool checkDiff = false ; 

		switch (menmRegressionType)
		{
			case LINEAR_EM:
				mobjLinearRegressionEM->CalculateRegressionFunction(*mptrVectRegressionPoints);
				break ; 
			default:
			case MIXTURE_REGRESSION:
				mobjMixtureRegressionEM->CalculateRegressionFunction(*mptrVectRegressionPoints);
				break ; 
				break ; 

		}
	}

	float clsRegressor::GetNETFromScan(float scan)
	{
		switch (menmRegressionType)
		{
			case LINEAR_EM:
				return (float) mobjLinearRegressionEM->GetPredictedValue(scan);
				break ; 
			case MIXTURE_REGRESSION:
				return (float) mobjMixtureRegressionEM->GetPredictedValue(scan);
				break ; 
			default:
				break ; 

		}
		return 0 ; 
	}


}