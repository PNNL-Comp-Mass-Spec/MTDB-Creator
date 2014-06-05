#region Namespaces

using System;
using NETPrediction;

#endregion

namespace MTDBFramework.Algorithms.RetentionTimePrediction
{
    public class KrokhinPredictor : IRetentionTimePredictor
    {
        private readonly iPeptideElutionTime m_predictor = new ElutionTimePredictionKrokhin();

        public double GetElutionTime(string peptide)
        {
            return Convert.ToDouble(m_predictor.GetElutionTime(peptide));
        }
    }
}
