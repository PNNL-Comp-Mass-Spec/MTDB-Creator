namespace MTDBFramework.Data
{
    public class XTandemTargetFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public XTandemTargetFilter(Options options)
        {
            FilterOptions = options;
        }

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