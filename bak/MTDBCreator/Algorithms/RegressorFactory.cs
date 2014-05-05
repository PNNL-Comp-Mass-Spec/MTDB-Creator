using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Algorithms
{
    public class RegressorFactory
    {
        public static IRegressionAlgorithm CreateRegressor(RegressorType type, RegressionTypeIdentifier identifier)
        {
            IRegressionAlgorithm regressor = null;
            switch (type)
            {
                case RegressorType.LcmsRegressor:
                    regressor = new LcmsRegressor(identifier);
                    break;
                default:
                    break;
            }

            return regressor;
        }
    }

    public enum RegressorType
    {
        LcmsRegressor
    }
}
