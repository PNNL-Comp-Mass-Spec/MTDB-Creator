#region Namespaces

using System;

#endregion

namespace Regressor.Algorithms
{
    public class RegressionPts : IComparable, IEquatable<RegressionPts>
    {
		public double MdblX; 
		public double MdblY;
        public double massError;
        public double netError; 

		public void Set(double x, double y)
		{
			MdblX = x ; 
			MdblY = y ; 
		}

        public void Set(double x, double mass_error, double net_eror)
        {
            MdblX = x;
            massError = mass_error;
            netError = net_eror;
        }

        public int CompareTo(Object obj)
        {
            var rp = obj as RegressionPts;
            return (rp == null ? 1 : this.MdblX.CompareTo(rp.MdblX));
        }

        public bool Equals(RegressionPts other)
        {
            return (MdblX == other.MdblX);
        }
    }
}
