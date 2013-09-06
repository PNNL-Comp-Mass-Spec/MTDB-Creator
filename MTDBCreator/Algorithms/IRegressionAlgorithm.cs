using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTDBCreator.Data;

namespace MTDBCreator.Algorithms
{
    /// <summary>
    /// Interface for regressing retention times
    /// </summary>
    public interface IRegressionAlgorithm
    {
        /// <summary>
        /// Calculates the regression from the observed to the basis
        /// </summary>
        /// <param name="observed"></param>
        /// <param name="basis"></param>
        /// <returns></returns>
        RegressionResult CalculateRegression(List<float> observed, List<float> basis);
        /// <summary>
        /// Aligns the list of targets based on the calculated regression model.
        /// </summary>
        /// <param name="targets"></param>                   
        void ApplyTransformation(List<Target> targets);
        /// <summary>
        /// Given a scan value, returns the transformed normalized elution time (NET).
        /// </summary>
        /// <param name="scan"></param>
        /// <returns></returns>
        double GetTransformedNET(int scan);
    }
}
