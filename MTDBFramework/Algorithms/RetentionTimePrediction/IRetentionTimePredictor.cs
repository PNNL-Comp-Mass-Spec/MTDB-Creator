namespace MTDBFramework.Algorithms.RetentionTimePrediction
{
    public interface IRetentionTimePredictor
    {
        /// <summary>
        /// Predicts the elution time of the clean peptide sequence.
        /// </summary>
        /// <param name="peptide">Clean peptide sequence</param>
        /// <returns>The elution time of the clean peptide sequence</returns>
        double GetElutionTime(string peptide);
    }
}
