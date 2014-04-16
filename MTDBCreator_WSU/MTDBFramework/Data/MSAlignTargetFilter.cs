namespace MTDBFramework.Data
{
    public class MsAlignTargetFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public MsAlignTargetFilter(Options options)
        {
            FilterOptions = options;
        }

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
