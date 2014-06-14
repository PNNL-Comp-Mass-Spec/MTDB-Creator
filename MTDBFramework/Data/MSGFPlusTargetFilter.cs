namespace MTDBFramework.Data
{
    public class MsgfPlusTargetFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public MsgfPlusTargetFilter(Options options)
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
            var result = evidence as MsgfPlusResult;

            if (result == null)
            {
                return true;
            }

            return ShouldFilter(result.QValue, result.SpecProb);
        }

        /// <summary>
        /// Determine whether the given evidence should be filtered out
        /// </summary>
        /// <param name="qValue">MSGF+ QValue (aka FDR)</param>
        /// <param name="specProb">MSGF+ Spectral Probability</param>
        /// <returns>True if the evidence should be filtered out (i.e. does not pass filters); false to keep it</returns>
        public bool ShouldFilter(double qValue, double specProb)
        {
            if (qValue > FilterOptions.MsgfFdr)
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
