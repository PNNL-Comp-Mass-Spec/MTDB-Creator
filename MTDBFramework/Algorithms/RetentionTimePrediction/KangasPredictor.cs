#region Namespaces

using System;
using NETPrediction;

#endregion

namespace MTDBFramework.Algorithms.RetentionTimePrediction
{
    public class KangasPredictor : IRetentionTimePredictor
    {
        private readonly iPeptideElutionTime m_kangas = new ElutionTimePredictionKangas();

        public double GetElutionTime(string peptide)
        {
            return Convert.ToDouble(m_kangas.GetElutionTime(peptide));
        }
    }
}
