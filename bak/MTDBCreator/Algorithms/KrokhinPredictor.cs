using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NETPrediction;

namespace MTDBCreator.Algorithms
{
    /// <summary>
    /// Predicts the peptide retention time for the given sequence
    /// </summary>
    public class KrokhinPredictor: IRetentionTimePredictor
    {
        private iPeptideElutionTime m_predictor;

        public KrokhinPredictor()
        {
            m_predictor = new ElutionTimePredictionKrokhin();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="peptide"></param>
        /// <returns></returns>
        public double GetElutionTime(string peptide)
        {
            return Convert.ToDouble(m_predictor.GetElutionTime(peptide));
        }        
    }
}
