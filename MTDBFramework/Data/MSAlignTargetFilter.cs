namespace MTDBFramework.Data
{
    public class MsAlignTargetFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public MsAlignTargetFilter(Options options)
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
            var result = evidence as MsAlignResult;

            if (result == null)
            {
                return true;
            }

            if (result.EScore > FilterOptions.MaxLogEValForMsAlignAlignment)
            {
                return true;
            }

            return false;
        }
    }
}
