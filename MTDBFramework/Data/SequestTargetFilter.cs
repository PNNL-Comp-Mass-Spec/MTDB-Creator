namespace MTDBFramework.Data
{
    public class SequestTargetFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public SequestTargetFilter(Options options)
        {
            FilterOptions = options;
        }

        /// <summary>
        /// Determine whether the given evidence should be filtered out
        /// </summary>
        /// <param name="evidence"></param>
        /// <returns>True if the evidence should be filtered out (i.e. does not pass filters); false to keep it</returns>
        public bool ShouldFilter(Evidence evidence)
        {
            var result = evidence as SequestResult;

            if (result == null)
            {
                return true;
            }

            if (result.XCorr < FilterOptions.MinXCorrForAlignment)
            {
                return true;
            }

            if (result.SpecProb > FilterOptions.MaxMsgfSpecProb)
            {
                return true;
            }

            return false;
        }
    }
}