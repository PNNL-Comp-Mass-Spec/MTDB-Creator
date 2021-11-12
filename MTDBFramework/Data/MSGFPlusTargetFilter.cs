using MTDBFrameworkBase.Data;

namespace MTDBFramework.Data
{
    /// <summary>
    /// Target Filtering for MSGF+ Workflows
    /// </summary>
    public class MsgfPlusTargetFilter : ITargetFilter
    {
        /// <summary>
        /// Options
        /// </summary>
        public Options FilterOptions { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public MsgfPlusTargetFilter(Options options)
        {
            FilterOptions = options;
        }

        /// <summary>
        /// Determine whether the given evidence should be filtered out
        /// </summary>
        /// <param name="evidence">Peptide evidence</param>
        /// <returns>True if the evidence should be filtered out (i.e. does not pass filters); false to keep it</returns>
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
            var passFilter = false;
            switch (FilterOptions.MsgfFilter)
            {
                case MsgfFilterType.Q_VALUE:
                    if (qValue > FilterOptions.MsgfQValue)
                    {
                        passFilter = true;
                    }
                    break;

                case MsgfFilterType.SPECTRAL_PROBABILITY:
                    if (specProb > FilterOptions.MaxMsgfSpecProb)
                    {
                        passFilter = true;
                    }
                    break;
            }

            return passFilter;
        }
    }
}
