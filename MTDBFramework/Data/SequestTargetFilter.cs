namespace MTDBFramework.Data
{
    public class SequestTargetFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public SequestTargetFilter(Options options)
        {
            FilterOptions = options;
        }

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

            return false;
        }
    }
}