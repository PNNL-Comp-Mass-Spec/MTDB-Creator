namespace MTDBFramework.Data
{
    public class XTandemTargetFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public XTandemTargetFilter(Options options)
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
            var result = evidence as XTandemResult;

            if (result == null)
            {
                return true;
            }

            return ShouldFilter(result.LogPeptideEValue, result.SpecProb);
        }

        /// <summary>
        /// Determine whether the given evidence should be filtered out
        /// </summary>
        /// <param name="logPepEValue">X!Tandem Log EValue</param>
        /// <param name="specProb">MSGF+ SpecProb</param>
        /// <returns>True if the evidence should be filtered out (i.e. does not pass filters); false to keep it</returns>
        public bool ShouldFilter(double logPepEValue, double specProb)
        {
            if (logPepEValue > FilterOptions.MaxLogEValForXTandemAlignment)
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