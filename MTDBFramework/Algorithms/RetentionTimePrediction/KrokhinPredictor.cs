#region Namespaces

using System;
using NETPrediction;

#endregion

namespace MTDBFramework.Algorithms.RetentionTimePrediction
{
    /// <summary>
    /// Normal Elution Time prediction using the KROKHIN algorithm
    /// </summary>
    public class KrokhinPredictor : IRetentionTimePredictor
    {
        private readonly iPeptideElutionTime m_predictor = new ElutionTimePredictionKrokhin();

        /// <summary>
        /// Calculate the Elution Time
        /// </summary>
        /// <param name="peptide"></param>
        /// <returns></returns>
        public double GetElutionTime(string peptide)
        {
            return Convert.ToDouble(m_predictor.GetElutionTime(peptide));
        }
    }
}
