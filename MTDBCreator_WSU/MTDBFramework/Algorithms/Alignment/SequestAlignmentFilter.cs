#region Namespaces

using MTDBFramework.Data;

#endregion

namespace MTDBFramework.Algorithms.Alignment
{
    public class SequestAlignmentFilter : ITargetFilter
    {
        public Options FilterOptions { get; set; }

        public SequestAlignmentFilter(Options options)
        {
            this.FilterOptions = options;
        }

        public bool ShouldFilter(Evidence evidence)
        {
            //if (evidence.IsSeqInfoExist != 1)
            //{
            //    return true;
            //}

            // In the alignment we will only use the unmodified peptides
            if (evidence.ModificationCount > this.FilterOptions.MaxModsForAlignment)
            {
                return true;
            }

            return false;
        }
    }
}
