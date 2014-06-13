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
        /// <param name="evidence"></param>
        /// <returns>True if the evidence should be filtered out (i.e. does not pass filters); false to keep it</returns>
        public bool ShouldFilter(Evidence evidence)
        {
            var result = evidence as MsgfPlusResult;

            if (result == null)
            {
                return true;
            }
            
            if(result.Sequence == null)
            {
                return true;
            }

            if (result.ModificationCount > FilterOptions.MaxModsForAlignment)
            {
                return true;
            }

            if(result.Fdr > FilterOptions.MsgfFdr)
            {
                return true;
            }

            if (result.SpecEValue > FilterOptions.MsgfSpectralEValue)
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
