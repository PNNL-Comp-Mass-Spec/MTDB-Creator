#region Namespaces

using System;
using NETPrediction;

#endregion

namespace MTDBFramework.Algorithms.RetentionTimePrediction
{
    /// <summary>
    /// Normal Elution Time prediction using the KANGAS algorithm
    /// </summary>
    public class KangasPredictor : IRetentionTimePredictor
    {
        private readonly iPeptideElutionTime m_kangas = new ElutionTimePredictionKangas();

        /// <summary>
        /// Calculate the Elution Time
        /// </summary>
        /// <param name="peptide"></param>
        /// <returns></returns>
        public double GetElutionTime(string peptide)
        {
            return Convert.ToDouble(m_kangas.GetElutionTime(peptide));
        }
    }
}
