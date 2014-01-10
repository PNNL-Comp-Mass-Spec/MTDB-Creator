namespace MTDBFramework.Data
{
    public class SequestTargetFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public SequestTargetFilter(Options options)
        {
            this.FilterOptions = options;
        }

        public bool ShouldFilter(Target target)
        {
            SequestResult result = target as SequestResult;

            if (result == null)
            {
                return true;
            }

            if (result.XCorr < this.FilterOptions.MinXCorrForAlignment)
            {
                return true;
            }

            return false;
        }
    }
}