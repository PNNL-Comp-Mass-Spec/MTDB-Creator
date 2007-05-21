#pragma once
#include <vector>
#include <math.h>
using namespace std ; 
namespace RegressionEngine
{
	class clsNormUnifEM
	{
		double mdbl_mean ; 
		double mdbl_var ;
		double mdbl_norm_fraction ; 
		int mint_num_iterations ; 
		vector<double> mvect_unif_prob ; 
	public:
		clsNormUnifEM(void);
		~clsNormUnifEM(void);
		void Reset() ; 
		void CalculateDistributions(vector<double> &vals) ;
		double GetStd() { return sqrt(mdbl_var) ; } 
		double GetMean() { return mdbl_mean ; } 
		double GetNormProb() { return mdbl_norm_fraction ; } 
	};
}