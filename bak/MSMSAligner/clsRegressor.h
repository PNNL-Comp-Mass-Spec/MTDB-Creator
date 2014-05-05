// clsUMCCreator.h
#pragma once
#include "clsLinearModelEM.h"
#include "clsMixtureModelRegressionEM.h"
#include <vector> 

#include "RegressionType.h"

using namespace System;
using namespace RegressionEngine;

namespace Regressor
{
	

	public ref class clsRegressor
	{

	private:
		RegressionType menmRegressionType ;
		bool mblnSuccessFull ; 
		RegressionEngine::clsLinearModelEM * mobjLinearRegressionEM ; 
		RegressionEngine::clsMixtureModelRegressionEM * mobjMixtureRegressionEM ; 
		std::vector<RegressionEngine::clsRegressionPts> * mptrVectRegressionPoints ; 
	public:
		clsRegressor() ; 
		~clsRegressor() ; 


		//void SetPoints(float (&x) __gc[], float (&y) __gc[]) ; 
		void clsRegressor::SetPoints(array<float> ^ %x, array<float> ^% y);
		void PerformRegression(RegressionType type) ; 

		property double Slope
		{
			double get()
			{
				switch (menmRegressionType)
				{
					case LINEAR_EM:
						return mobjLinearRegressionEM->mdbl_slope ; 
						break ; 
					case MIXTURE_REGRESSION:
						return mobjMixtureRegressionEM->GetSlope() ;
						break ; 
					default:
						return 20 ; 
						break ; 

				}
			}
		}
		property double Intercept
		{
			double get()
			{
			switch (menmRegressionType)
			{
				case LINEAR_EM:
					return mobjLinearRegressionEM->mdbl_intercept ; 
					break ; 
				case MIXTURE_REGRESSION:
					return mobjMixtureRegressionEM->GetIntercept() ;
					break ; 
				default:
					return 20 ; 
					break ; 

			}
			}
		}

		property short RegressionOrder
		{
			short get()
			{
				switch (menmRegressionType)
				{
					case LINEAR_EM:
						return 0 ; 
						break ; 
					case MIXTURE_REGRESSION:
						return mobjMixtureRegressionEM->GetRegressionOrder() ;
						break ; 
					default:
						return 20 ; 
						break ; 

				}
			}
			void set(short order)
			{
				switch (menmRegressionType)
				{
					case LINEAR_EM:
						return  ; 
						break ; 
					case MIXTURE_REGRESSION:
						return mobjMixtureRegressionEM->SetRegressionOrder(order) ;
						break ; 
					default:
						break ; 
				}
				return ; 
			}
		}

		property double RSquared
		{
			double get()
			{
				switch (menmRegressionType)
				{
					case LINEAR_EM:
						return mobjLinearRegressionEM->mdbl_rsq ; 
						break ; 
					case MIXTURE_REGRESSION:
						return 0 ;
						break ; 
					default:
						return 20 ; 
						break ; 

				}
				return 0 ; 
			}
		}

		property int PercentComplete
		{
			int get()
			{
				switch (menmRegressionType)
				{
					case LINEAR_EM:
						return mobjLinearRegressionEM->PercentComplete() ;
						break ; 
					case MIXTURE_REGRESSION:
						return mobjMixtureRegressionEM->PercentComplete() ;
						break ; 
					default:
						return 20 ; 
						break ; 

				}
				return 10 ; 
			}
		}

		float GetNETFromScan(float scan) ; 
	};
}
