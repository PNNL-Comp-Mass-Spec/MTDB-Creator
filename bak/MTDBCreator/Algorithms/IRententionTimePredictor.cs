using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTDBCreator.Algorithms
{
    /// <summary>
    /// Predicts the retention time for a given peptide.
    /// </summary>
    public interface IRetentionTimePredictor
    {
        /// <summary>
        /// Predicts the elution time of the object.
        /// </summary>
        /// <param name="peptide"></param>
        /// <returns></returns>
        double GetElutionTime(string peptide);
    }
}
