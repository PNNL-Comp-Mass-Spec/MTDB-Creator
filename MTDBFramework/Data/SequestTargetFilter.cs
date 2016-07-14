namespace MTDBFramework.Data
{
    /// <summary>
    /// Target filtering for Sequest workflows
    /// </summary>
    public class SequestTargetFilter : ITargetFilter
    {
        /// <summary>
        /// Options
        /// </summary>
        public Options FilterOptions { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public SequestTargetFilter(Options options)
        {
            FilterOptions = options;
        }

        /// <summary>
        /// Determine whether the given evidence should be filtered out
        /// </summary>
        /// <param name="evidence">Peptide evidence</param>
        /// /// <returns>True if the evidence should be filtered out (i.e. does not pass filters); false to keep it</returns>
        public bool ShouldFilter(Evidence evidence)
        {
            var result = evidence as SequestResult;

            if (result == null)
            {
                return true;
            }

            return ShouldFilter(result.XCorr, result.SpecProb);
        }

        /// <summary>
        /// Determine whether the given evidence should be filtered out
        /// </summary>
        /// <param name="xcorr">Sequest XCorr</param>
        /// /// <param name="specProb">MSGF+ SpecProb</param>
        /// <returns>True if the evidence should be filtered out (i.e. does not pass filters); false to keep it</returns>
        public bool ShouldFilter(double xcorr, double specProb)
        {
            if (xcorr < FilterOptions.MinXCorrForAlignment)
            {
                return true;
            }

            if (specProb > FilterOptions.MaxMsgfSpecProb)
            {
                return true;
            }

            return false;
        }
    }
}