using System;
using System.Collections.Generic;
using System.Text;

namespace MTDBCreator
{

    /// <summary>
    /// Result from regression analysis.
    /// </summary>
    public class RegressionResult
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="datasetName"></param>
        /// <param name="netSlope"></param>
        /// <param name="netIntercept"></param>
        /// <param name="netRSquared"></param>
        /// <param name="uniqueMassTags"></param>
        public RegressionResult(string datasetName,
                                double netSlope,
                                double netIntercept,
                                double netRSquared,
                                int uniqueMassTags)
        {
            DatasetName          = datasetName;
            NETSlope             = netSlope;
            NETRSquared          = netRSquared;
            NETIntercept         = netIntercept;
            NumberUniqueMassTags = uniqueMassTags;
        }

        public RegressionResult()
        {
            DatasetName          = "";
            NETSlope             = 0;
            NETRSquared          = 0;
            NETIntercept         = 0;
            NumberUniqueMassTags = 0;
        }


        public string DatasetName { get; set; }
        public double NETSlope { get; set; }
        public double NETIntercept { get; set; }
        public double NETRSquared { get; set; }
        public int NumberUniqueMassTags { get; set; }
    }

}
