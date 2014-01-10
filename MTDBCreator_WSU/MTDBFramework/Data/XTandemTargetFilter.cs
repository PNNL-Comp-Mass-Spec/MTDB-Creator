namespace MTDBFramework.Data
{
    public class XTandemTargetFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public XTandemTargetFilter(Options options)
        {
            this.FilterOptions = options;
        }

        public bool ShouldFilter(Target target)
        {
            XTandemResult result = target as XTandemResult;

            if (result == null)
            {
                return true;
            }

            if (result.LogPeptideEValue > this.FilterOptions.MaxLogEValForXTandemAlignment)
            {
                return true;
            }

            return false;
        }
    }
}