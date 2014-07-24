namespace MTDBFramework.Algorithms.RetentionTimePrediction
{
	/// <summary>
	/// Interface for Elution time prediction of a clean peptide sequence
	/// </summary>
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
