#pragma once

#pragma once 
#pragma managed(push)
#pragma unmanaged 

namespace RegressionEngine
{
	class clsRegressionPts
	{
	public:
		double mdbl_x ; 
		double mdbl_y ; 
		void Set(double x, double y)
		{
			mdbl_x = x ; 
			mdbl_y = y ; 
		}
	} ; 
}

#pragma managed(pop)