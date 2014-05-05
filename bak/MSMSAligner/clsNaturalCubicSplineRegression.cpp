
#include <algorithm>
#include ".\clsNaturalCubicSplineRegression.h"
#include "Matrix.h" 
#include <float.h>
#include <fstream>
#include <iostream> 
#include <math.h>
using namespace std ; 

namespace RegressionEngine
{

	void clsNaturalCubicSplineRegression::Clear()
	{
		mvect_pts.clear() ; 
		mvect_interval_start.clear() ; 
	}

	void clsNaturalCubicSplineRegression::PreprocessCopyData(vector<clsRegressionPts> vect_pts)
	{

		// first find minimum and maximums
		int num_pts = vect_pts.size() ; 

		mdbl_min_x = DBL_MAX ; 
		mdbl_max_x = -1 * DBL_MAX ; 
		for (int pt_num = 0 ; pt_num < num_pts ; pt_num++)
		{
			clsRegressionPts pt = vect_pts[pt_num] ; 
			if (pt.mdbl_x < mdbl_min_x)
				mdbl_min_x = pt.mdbl_x ; 
			if (pt.mdbl_x > mdbl_max_x)
				mdbl_max_x = pt.mdbl_x ; 
		}
		mvect_pts.insert(mvect_pts.begin(), vect_pts.begin(), vect_pts.end()) ; 

		for (int i = 0 ; i <= mint_num_knots ; i++)
		{
			double val = (i * (mdbl_max_x - mdbl_min_x)) / (mint_num_knots+1) + mdbl_min_x;
			mvect_interval_start.push_back(val) ; 
		}
	}


	bool clsNaturalCubicSplineRegression::CalculateLSQRegressionCoefficients(vector<clsRegressionPts> &vect_pts, int numIterations)
	{
		vector<clsRegressionPts> vect_pts_copy ; 
		vect_pts_copy.insert(vect_pts_copy.begin(), vect_pts.begin(), vect_pts.end()) ; 

		for (int iterationNum = 0 ; iterationNum < numIterations ; iterationNum++)
		{
			if (vect_pts_copy.size() == 0)
				return false ; 
			CalculateLSQRegressionCoefficients(vect_pts_copy) ; 
			vect_pts_copy.clear() ; 
			int numPts = vect_pts.size() ; 
			double sum_diff = 0 ; 

			for (int ptNum = 0 ; ptNum < numPts ; ptNum++)
			{
				clsRegressionPts pt = vect_pts[ptNum] ; 
				sum_diff += pt.mdbl_y - GetPredictedValue(pt.mdbl_x) ; 
			}
			double mean = sum_diff / numPts ; 
			double sum_sq_diff = 0 ; 
			for (int ptNum = 0 ; ptNum < numPts ; ptNum++)
			{
				clsRegressionPts pt = vect_pts[ptNum] ; 
				double predY = GetPredictedValue(pt.mdbl_x) ; 
				sum_sq_diff += (pt.mdbl_y - predY - mean ) * (pt.mdbl_y - predY - mean )  ; 
			}
			double std_dev = sqrt(sum_sq_diff/(numPts-1)) ; 
			for (int ptNum = 0 ; ptNum < numPts ; ptNum++)
			{
				clsRegressionPts pt = vect_pts[ptNum] ; 
				if (abs(pt.mdbl_y - GetPredictedValue(pt.mdbl_x)) < 5 * std_dev)
				{
					vect_pts_copy.push_back(pt) ; 
				}
			}
		}
	}

	// input points are [x, y].
	// order specifies order of regression line. order = 0 is constant, order = 1 is linear, order = 2 is quadratic,
	// and so on.. maximum order supported is MAX_ORDER.
	bool clsNaturalCubicSplineRegression::CalculateLSQRegressionCoefficients(vector<clsRegressionPts> &vect_pts)
	{
		Clear() ; 
		if (mint_num_knots < 2)
			// need at least two knots for a natural cubic spline.
			return false ; 

		if (vect_pts.size() == 0)
			return false ; 

		PreprocessCopyData(vect_pts) ; 

		MATRIX *A, *b, *c, *b_interp ; 
		MATRIX *Atranspose; 
		MATRIX *Aintermediate1, *Aintermediate2, *Aintermediate3 ; 


		int num_pts = mvect_pts.size() ; 

		A = matrix_allocate(num_pts, mint_num_knots, sizeof(double)) ; 
		b = matrix_allocate(num_pts, 1, sizeof(double)) ; 

		double **ptr_matrix_A = (double **) A->ptr ;
		double **ptr_matrix_b = (double **) b->ptr ;


		double interval_width = (mdbl_max_x - mdbl_min_x) /(mint_num_knots+1) ;

		for (int pt_num = 0 ; pt_num < num_pts ; pt_num++)
		{
			clsRegressionPts pt = mvect_pts[pt_num] ; 
			double coeff = 1 ; 
			ptr_matrix_A[pt_num][0] = coeff ; 
			ptr_matrix_A[pt_num][1] = pt.mdbl_x ; 

			int interval_num = ((pt.mdbl_x - mdbl_min_x) * (mint_num_knots+1)) / (mdbl_max_x - mdbl_min_x) ; 
			if (interval_num > mint_num_knots)
				interval_num = mint_num_knots ; 

			double d_Kminus1 = 0 ; 
			if (pt.mdbl_x > mvect_interval_start[mint_num_knots-1])
			{
				d_Kminus1 = pow(pt.mdbl_x - mvect_interval_start[mint_num_knots-1], 3) ; 
				if (pt.mdbl_x > mvect_interval_start[mint_num_knots])
					d_Kminus1 -= pow(pt.mdbl_x - mvect_interval_start[mint_num_knots], 3) ; 
				d_Kminus1 /= interval_width ; 
			}

			for (int k = 1 ; k <= mint_num_knots-2 ; k++)
			{
				double d_kminus1 = 0 ; 
				if (pt.mdbl_x > mvect_interval_start[k])
				{
					d_kminus1 = pow(pt.mdbl_x - mvect_interval_start[k], 3) ; 
					if (pt.mdbl_x > mvect_interval_start[mint_num_knots])
						d_kminus1 -= pow(pt.mdbl_x - mvect_interval_start[mint_num_knots], 3) ; 
					d_kminus1 /= interval_width ; 
				}

				ptr_matrix_A[pt_num][k+1] = d_kminus1 - d_Kminus1 ; 
			}

			ptr_matrix_b[pt_num][0] = pt.mdbl_y ; 
		}

			
		PrintMatrix(A, "c:\\A.csv") ; 
		Atranspose = matrix_transpose(A) ; 
		Aintermediate1 = matrix_mult(Atranspose, A) ;
		PrintMatrix(Aintermediate1, "c:\\intermediate1.csv") ; 
		Aintermediate2 = matrix_invert(Aintermediate1) ; 
		if (Aintermediate2 == NULL)
		{
			return false ; 
		}
		Aintermediate3 = matrix_mult(Aintermediate2, Atranspose) ; 

		c = matrix_mult(Aintermediate3, b) ; 
		b_interp = matrix_mult(A, c) ; 

		for (int col_num = 0 ; col_num < mint_num_knots ; col_num++)
		{
			marr_coeffs[col_num] = *((double *)&c->ptr[col_num][0]) ; 
		}


//		PrintPoints("c:\\pts_nat.csv") ; 
		matrix_free(b) ; 
		matrix_free(c) ; 
		matrix_free(Aintermediate1) ; 
		matrix_free(Aintermediate2) ; 
		matrix_free(Aintermediate3) ; 
		matrix_free(Atranspose) ; 
		matrix_free(A); 
		return true ; 
	}

	void clsNaturalCubicSplineRegression::PrintRegressionFunction(char *file_name)
	{
	}

	double clsNaturalCubicSplineRegression::GetPredictedValue(double x) 
	{
		if (mvect_pts.size() == 0)
			return 0 ; 

		if (x <= mdbl_min_x)
		{
			return marr_coeffs[0] + marr_coeffs[1] * mdbl_min_x ; 
		}
		if (x >= mdbl_max_x)
		{
			x = mdbl_max_x ; 
		}

		double val = marr_coeffs[0] ; 
		double interval_width = (mdbl_max_x - mdbl_min_x) /(mint_num_knots+1) ;

		val = marr_coeffs[0] + marr_coeffs[1] * x ;  

		double d_Kminus1 = 0 ; 
		if (x > mvect_interval_start[mint_num_knots-1])
		{
			d_Kminus1 = pow(x - mvect_interval_start[mint_num_knots-1], 3) ; 
			if (x > mvect_interval_start[mint_num_knots])
				d_Kminus1 -= pow(x - mvect_interval_start[mint_num_knots], 3) ; 
			d_Kminus1 /= interval_width ; 
		}

		for (int k = 1 ; k <= mint_num_knots-2 ; k++)
		{
			double d_kminus1 = 0 ; 
			if (x > mvect_interval_start[k])
			{
				d_kminus1 = pow(x - mvect_interval_start[k], 3) ; 
				if (x > mvect_interval_start[mint_num_knots])
					d_kminus1 -= pow(x - mvect_interval_start[mint_num_knots], 3) ; 
				d_kminus1 /= interval_width ; 
			}

			val += (d_kminus1 - d_Kminus1) * marr_coeffs[k+1] ; 
		}

		return val ; 
	}

	void clsNaturalCubicSplineRegression::PrintPoints(char *file_name)
	{
		ofstream fout(file_name) ;
		int num_pts = mvect_pts.size() ; 
		for (int pt_num = 0 ; pt_num < num_pts ; pt_num++)
		{
			clsRegressionPts pt = mvect_pts[pt_num] ; 
			fout<<pt.mdbl_x<<","<<pt.mdbl_y<<","<<pt.mdbl_y - GetPredictedValue(pt.mdbl_x)<<"\n" ; 
		}
		fout.close() ; 
	}


}