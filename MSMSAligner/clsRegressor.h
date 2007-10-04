// clsUMCCreator.h
#pragma once
#include "clsLinearModelEM.h"
#include "clsMixtureModelRegressionEM.h"
#include <vector> 

using namespace System;
namespace Regressor
{
	public __gc class clsRegressor
	{
	public:
		__value enum RegressionType {LINEAR_EM = 0, MIXTURE_REGRESSION } ; 
	private:
		RegressionType menmRegressionType ;
		bool mblnSuccessFull ; 
		RegressionEngine::clsLinearModelEM __nogc *mobjLinearRegressionEM ; 
		RegressionEngine::clsMixtureModelRegressionEM __nogc *mobjMixtureRegressionEM ; 
		std::vector<RegressionEngine::clsRegressionPts> __nogc *mptrVectRegressionPoints ; 
	public:
		clsRegressor() ; 
		~clsRegressor() ; 


		void SetPoints(float (&x) __gc[], float (&y) __gc[]) ; 
		void PerformRegression(RegressionType type) ; 

		__property double get_Slope()
		{
			; 
			switch (menmRegressionType)
			{
				case RegressionType::LINEAR_EM:
					return mobjLinearRegressionEM->mdbl_slope ; 
					break ; 
				case RegressionType::MIXTURE_REGRESSION:
					return mobjMixtureRegressionEM->GetSlope() ;
					break ; 
				default:
					return 20 ; 
					break ; 

			}
		}
		__property double get_Intercept()
		{
			switch (menmRegressionType)
			{
				case RegressionType::LINEAR_EM:
					return mobjLinearRegressionEM->mdbl_intercept ; 
					break ; 
				case RegressionType::MIXTURE_REGRESSION:
					return mobjMixtureRegressionEM->GetIntercept() ;
					break ; 
				default:
					return 20 ; 
					break ; 

			}
		}

		__property short get_RegressionOrder()
		{
			switch (menmRegressionType)
			{
				case RegressionType::LINEAR_EM:
					return 0 ; 
					break ; 
				case RegressionType::MIXTURE_REGRESSION:
					return mobjMixtureRegressionEM->GetRegressionOrder() ;
					break ; 
				default:
					return 20 ; 
					break ; 

			}
		}
		__property void set_RegressionOrder(short order)
		{
			switch (menmRegressionType)
			{
				case RegressionType::LINEAR_EM:
					return  ; 
					break ; 
				case RegressionType::MIXTURE_REGRESSION:
					return mobjMixtureRegressionEM->SetRegressionOrder(order) ;
					break ; 
				default:
					break ; 
			}
			return ; 
		}

		__property double get_RSquared()
		{
			switch (menmRegressionType)
			{
				case RegressionType::LINEAR_EM:
					return mobjLinearRegressionEM->mdbl_rsq ; 
					break ; 
				case RegressionType::MIXTURE_REGRESSION:
					return 0 ;
					break ; 
				default:
					return 20 ; 
					break ; 

			}
			return 0 ; 
		}

		__property int get_PercentComplete()
		{
			switch (menmRegressionType)
			{
				case RegressionType::LINEAR_EM:
					return mobjLinearRegressionEM->PercentComplete() ;
					break ; 
				case RegressionType::MIXTURE_REGRESSION:
					return mobjMixtureRegressionEM->PercentComplete() ;
					break ; 
				default:
					return 20 ; 
					break ; 

			}
			return 10 ; 
		}

		float GetNETFromScan(float scan) ; 
	};
}
