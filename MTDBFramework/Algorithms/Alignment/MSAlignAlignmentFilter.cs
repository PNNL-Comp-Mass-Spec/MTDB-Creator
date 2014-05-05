using MTDBFramework.Data;

namespace MTDBFramework.Algorithms.Alignment
{
    public class MsAlignAlignmentFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public MsAlignAlignmentFilter(Options options)
        {
            FilterOptions = options;
        }

        public bool ShouldFilter(Evidence evidence)
        {
            // In the alignment we will only use the unmodified peptides
            if (evidence.ModificationCount > FilterOptions.MaxModsForAlignment)
            {
                return true;
            }

            return false;
        }
    }
}
