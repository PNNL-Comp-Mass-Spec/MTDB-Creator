#region Namespaces

using System;

#endregion

namespace Regressor.Algorithms
{
    public class RegressionPts : IComparable, IEquatable<RegressionPts>
    {
		public double MdblX; 
		public double MdblY;
        public double MassError;
        public double NetError; 

		public void Set(double x, double y)
		{
			MdblX = x ; 
			MdblY = y ; 
		}

        public void Set(double x, double massError, double netError)
        {
            MdblX = x;
            MassError = massError;
            NetError = netError;
        }

        public int CompareTo(Object obj)
        {
            var rp = obj as RegressionPts;
            return (rp == null ? 1 : this.MdblX.CompareTo(rp.MdblX));
        }

        public bool Equals(RegressionPts other)
        {
            return (Math.Abs(MdblX - other.MdblX) < double.Epsilon);
        }
    }
}
