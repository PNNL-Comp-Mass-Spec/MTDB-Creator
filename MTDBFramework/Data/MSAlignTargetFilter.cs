namespace MTDBFramework.Data
{
	/// <summary>
	/// Target filtering for MSAlign Workflows
	/// </summary>
    public class MsAlignTargetFilter : ITargetFilter
    {
		/// <summary>
		/// Options
		/// </summary>
        public Options FilterOptions { get; set; }
       
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options"></param>
        public MsAlignTargetFilter(Options options)
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
            var result = evidence as MsAlignResult;

            if (result == null)
            {
                return true;
            }

            return ShouldFilter(result.EValue, result.SpecProb);
        }

        /// <summary>
        /// Determine whether the given evidence should be filtered out
        /// </summary>
        /// <param name="eValue">MSAlign EValue</param>
        /// <param name="specProb">MSGF Spectral Probability</param>
        /// <returns>True if the evidence should be filtered out (i.e. does not pass filters); false to keep it</returns>
        public bool ShouldFilter(double eValue, double specProb)
        {
            if (eValue > FilterOptions.MaxLogEValForMsAlignAlignment)
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
