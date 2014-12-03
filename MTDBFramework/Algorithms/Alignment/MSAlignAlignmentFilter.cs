using MTDBFramework.Data;

namespace MTDBFramework.Algorithms.Alignment
{
	/// <summary>
	/// Filter information for MSAlign format
	/// </summary>
    public class MsAlignAlignmentFilter : ITargetFilter
    {
		/// <summary>
		/// Options
		/// </summary>
        public Options FilterOptions { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="options"></param>
        public MsAlignAlignmentFilter(Options options)
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
            if (evidence.ModificationCount > FilterOptions.MaxModsForAlignment)
            {
                return true;
            }

            return false;
        }
    }
}
