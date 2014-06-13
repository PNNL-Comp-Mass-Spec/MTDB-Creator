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
        /// <param name="evidence"></param>
        /// <returns>True if the evidence should be filtered out (i.e. does not pass filters); false to keep it</returns>
        public bool ShouldFilter(Evidence evidence)
        {
            var result = evidence as XTandemResult;

            if (result == null)
            {
                return true;
            }

            if (result.LogPeptideEValue > FilterOptions.MaxLogEValForXTandemAlignment)
            {
                return true;
            }

            return false;
        }
    }
}